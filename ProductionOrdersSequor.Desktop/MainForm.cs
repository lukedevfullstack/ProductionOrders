using ProductionOrdersSequor.Desktop.Forms;
using ProductionOrdersSequor.Desktop.Services;

namespace ProductionOrdersSequor.Desktop
{
    public partial class MainForm : Form
    {
        private TextBox txtEmail = null!;
        private ComboBox cmbOrders = null!;
        private Label lblProductInfo = null!;
        private Label lblTimer = null!;
        private TextBox txtQuantity = null!;
        private ComboBox cmbMaterials = null!;
        private Button btnSubmit = null!;
        private Button btnHistory = null!;
        private System.Windows.Forms.Timer timerCycle = null!;
        private PictureBox picProduct = null!;

        private List<ProductionOrderDto> _orders = new();
        private ProductionOrderDto? _selectedOrder;
        private int _elapsedSeconds = 0;

        public MainForm()
        {
            InitializeComponentCustom();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "Monitoramento e Apontamento - Sequor";
            this.Size = new Size(500, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            // 1. E-mail do Operador
            var lblEmail = new Label { Text = "Identificação do Operador (E-mail):", Location = new Point(20, 15), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(20, 38), Width = 440, BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White };
            txtEmail.TextChanged += (s, e) => ValidateForm();

            // 2. Ordem de Produção
            var lblOrders = new Label { Text = "Ordem de Produção:", Location = new Point(20, 75), AutoSize = true };
            cmbOrders = new ComboBox { Location = new Point(20, 98), Width = 440, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbOrders.SelectedIndexChanged += CmbOrders_SelectedIndexChanged;

            // 3. Imagem e Informações do Produto
            picProduct = new PictureBox
            {
                Location = new Point(20, 140),
                Size = new Size(60, 60),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(40, 40, 40)
            };

            lblProductInfo = new Label
            {
                Text = "",
                Location = new Point(90, 140),
                Width = 370,
                Height = 60,
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.LightGray
            };

            lblTimer = new Label { Text = "⏱️ Tempo decorrido: 0s", Location = new Point(20, 215), Width = 440, AutoSize = true, ForeColor = Color.Gold };

            // 4. Quantidade
            var lblQty = new Label { Text = "Quantidade:", Location = new Point(20, 250), AutoSize = true };
            txtQuantity = new TextBox { Location = new Point(20, 273), Width = 440, BackColor = Color.FromArgb(45, 45, 45), ForeColor = Color.White };
            txtQuantity.TextChanged += (s, e) => ValidateForm();

            // 5. Material Utilizado
            var lblMat = new Label { Text = "Material Utilizado:", Location = new Point(20, 312), AutoSize = true };
            cmbMaterials = new ComboBox { Location = new Point(20, 335), Width = 440, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
            cmbMaterials.SelectedIndexChanged += (s, e) => ValidateForm();

            // 6. Botão de Envio
            btnSubmit = new Button
            {
                Text = "Enviar Apontamento",
                Location = new Point(20, 395),
                Width = 210,
                Height = 45,
                BackColor = Color.FromArgb(85, 85, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnSubmit.Click += BtnSubmit_Click;

            // Botão de Histórico (inicialmente desativado até preencher o e-mail)
            btnHistory = new Button
            {
                Text = "📋 Ver Histórico",
                Location = new Point(250, 395),
                Width = 210,
                Height = 45,
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnHistory.Click += (s, e) =>
            {
                string email = txtEmail.Text.Trim();
                var historyForm = new HistoryForm(email);
                historyForm.ShowDialog();
            };

            // 7. Timer de Ciclo
            timerCycle = new System.Windows.Forms.Timer { Interval = 1000 };
            timerCycle.Tick += TimerCycle_Tick;

            // Adicionar controlos à Tela
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblOrders);
            this.Controls.Add(cmbOrders);
            this.Controls.Add(picProduct);
            this.Controls.Add(lblProductInfo);
            this.Controls.Add(lblTimer);
            this.Controls.Add(lblQty);
            this.Controls.Add(txtQuantity);
            this.Controls.Add(lblMat);
            this.Controls.Add(cmbMaterials);
            this.Controls.Add(btnSubmit);
            this.Controls.Add(btnHistory);

            this.Load += MainForm_Load;
        }

        private void ValidateForm()
        {
            bool hasEmail = !string.IsNullOrWhiteSpace(txtEmail.Text);
            bool hasOrder = _selectedOrder != null;
            int requiredCycle = _selectedOrder?.CycleTime ?? 0;
            bool hasValidCycle = _selectedOrder != null && _elapsedSeconds >= requiredCycle;
            bool hasQuantity = decimal.TryParse(txtQuantity.Text, out var qty) && qty > 0;
            bool hasMaterial = cmbMaterials.SelectedIndex != -1;

            bool isFormValid = hasEmail && hasOrder && hasValidCycle && hasQuantity && hasMaterial;

            // Validação do Botão de Envio
            btnSubmit.Enabled = isFormValid;
            btnSubmit.BackColor = isFormValid ? Color.FromArgb(40, 167, 69) : Color.FromArgb(85, 85, 85);

            // Validação do Botão de Histórico (só clivável se houver e-mail)
            btnHistory.Enabled = hasEmail;
            btnHistory.BackColor = hasEmail ? Color.FromArgb(70, 70, 70) : Color.FromArgb(50, 50, 50);
        }

        private async void BtnSubmit_Click(object? sender, EventArgs e)
        {
            try
            {
                var payload = new ProductionPayloadDto
                {
                    Email = txtEmail.Text.Trim(),
                    Order = _selectedOrder!.Order,
                    ProductionDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    ProductionTime = DateTime.Now.ToString("HH:mm:ss"),
                    Quantity = decimal.Parse(txtQuantity.Text),
                    MaterialCode = cmbMaterials.SelectedValue?.ToString() ?? string.Empty,
                    CycleTime = _elapsedSeconds
                };

                HttpResponseMessage response = await ApiClient.SendProductionAsync(payload);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                using var doc = System.Text.Json.JsonDocument.Parse(jsonResponse);
                var root = doc.RootElement;

                string type = root.TryGetProperty("type", out var tProp) ? tProp.GetString() ?? "" : "";
                int status = root.TryGetProperty("status", out var sProp) ? sProp.GetInt32() : (int)response.StatusCode;
                string description = root.TryGetProperty("description", out var dProp) ? dProp.GetString() ?? "" : (response.ReasonPhrase ?? "");

                if (!response.IsSuccessStatusCode || type == "E" || status != 200)
                {
                    MessageBox.Show(
                        $"Não foi possível realizar o apontamento:\n{description}",
                        "Falha no Apontamento",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // 4. Sucesso: Exibir mensagem e limpar o formulário
                MessageBox.Show("Apontamento enviado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpar dados do formulário
                txtQuantity.Clear();
                cmbOrders.SelectedIndex = -1;
                cmbMaterials.DataSource = null;
                cmbMaterials.Enabled = false;
                lblProductInfo.Text = string.Empty;
                picProduct.Image = null;
                _selectedOrder = null;
                _elapsedSeconds = 0;
                timerCycle.Stop();
                lblTimer.Text = "⏱️ Tempo decorrido: 0s";
                ValidateForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro na comunicação com o servidor: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void MainForm_Load(object? sender, EventArgs e) => await LoadOrdersAsync();

        private async Task LoadOrdersAsync()
        {
            _orders = await ApiClient.GetOrdersAsync();
            cmbOrders.DataSource = _orders;
            cmbOrders.DisplayMember = "Order";
            cmbOrders.ValueMember = "Order";
            cmbOrders.SelectedIndex = -1;
        }

        private void CmbOrders_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbOrders.SelectedItem is ProductionOrderDto order)
            {
                _selectedOrder = order;
                _elapsedSeconds = 0;
                timerCycle.Start();

                cmbMaterials.DataSource = _selectedOrder.Materials;
                cmbMaterials.DisplayMember = "MaterialDescription";
                cmbMaterials.ValueMember = "MaterialCode";
                cmbMaterials.Enabled = true;

                lblProductInfo.Text = $" Produto: {_selectedOrder.ProductDescription} ({_selectedOrder.ProductCode})";

                if (!string.IsNullOrEmpty(_selectedOrder.Image))
                {
                    string imageUrl = _selectedOrder.Image.StartsWith("http")
                        ? _selectedOrder.Image
                        : $"https://localhost:7079/{_selectedOrder.Image.TrimStart('/')}";

                    try { picProduct.LoadAsync(imageUrl); } catch { picProduct.Image = null; }
                }
                else
                {
                    picProduct.Image = null;
                }
            }
            else
            {
                _selectedOrder = null;
                timerCycle.Stop();
                _elapsedSeconds = 0;
                cmbMaterials.DataSource = null;
                cmbMaterials.Enabled = false;
                lblProductInfo.Text = string.Empty;
                lblTimer.Text = "⏱️ Tempo decorrido: 0s";
                picProduct.Image = null;
            }

            ValidateForm();
        }

        private void TimerCycle_Tick(object? sender, EventArgs e)
        {
            _elapsedSeconds++;
            if (_selectedOrder != null)
            {
                int requiredCycle = _selectedOrder?.CycleTime ?? 0;
                bool isValidCycle = _elapsedSeconds >= requiredCycle;
                lblTimer.Text = $"⏱️ Tempo decorrido: {_elapsedSeconds}s / {requiredCycle}s {(isValidCycle ? "✔️ Liberado" : "⏳ Aguardando ciclo...")}";
                lblTimer.ForeColor = isValidCycle ? Color.LightGreen : Color.Gold;

                ValidateForm();
            }
        }
    }
}