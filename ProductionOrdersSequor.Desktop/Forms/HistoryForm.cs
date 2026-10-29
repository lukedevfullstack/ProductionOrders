using ProductionOrdersSequor.Desktop.Services;

namespace ProductionOrdersSequor.Desktop.Forms
{
    public partial class HistoryForm : Form
    {
        private DataGridView dgvHistory = null!;
        private Button btnRefresh = null!;
        private readonly string _operatorEmail;

        public HistoryForm(string operatorEmail)
        {
            _operatorEmail = operatorEmail;
            InitializeComponentCustom();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "Histórico de Apontamentos - Sequor";
            this.Size = new Size(850, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            btnRefresh = new Button
            {
                Text = "🔄 Atualizar Histórico",
                Location = new Point(20, 20),
                Width = 180,
                Height = 35,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.Click += async (s, e) => await LoadHistoryAsync();

            dgvHistory = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(790, 390),
                BackgroundColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                GridColor = Color.FromArgb(60, 60, 60),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false // Permitir definir colunas manuais para controlo total
            };

            // Definição manual e limpa das colunas
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Order", HeaderText = "Ordem", Width = 80 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProductionDate", HeaderText = "Data", Width = 95 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProductionTime", HeaderText = "Hora", Width = 85 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Quantity", HeaderText = "Quantidade", Width = 90 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaterialDescription", HeaderText = "Material", Width = 80 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CycleTime", HeaderText = "Ciclo (s)", Width = 80 });

            // Estilização do cabeçalho
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvHistory.EnableHeadersVisualStyles = false;

            dgvHistory.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            dgvHistory.DefaultCellStyle.ForeColor = Color.White;
            dgvHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);

            this.Controls.Add(btnRefresh);
            this.Controls.Add(dgvHistory);

            this.Load += async (s, e) => await LoadHistoryAsync();
        }

        private async Task LoadHistoryAsync()
        {
            btnRefresh.Enabled = false;
            try
            {
                List<ProductionHistoryDto> history = await ApiClient.GetHistoryAsync(_operatorEmail);

                dgvHistory.DataSource = history;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar histórico: {ex.Message}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {

        }
    }
}