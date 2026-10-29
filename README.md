# 🏭 Production Orders Sequor

Sistema corporativo para gerenciamento, monitoramento em tempo real e apontamento de ordens de produção. O ecossistema é composto por uma **API Backend em ASP.NET Core** e um **Client Desktop de alta performance em .NET 8 (Windows Forms)** com suporte nativo a *Dark Mode*.

---

## 🚀 Funcionalidades Principais

* **Monitoramento de Ciclo em Tempo Real:** Temporizador integrado (`TimerCycle`) que valida o tempo decorrido do ciclo produtivo antes de habilitar o envio do apontamento.
* **Validação de Formulário Dinâmica:** Os botões de ação e submissão adaptam-se com base na integridade dos dados inseridos (e-mail do operador, seleção de ordem, quantidade e material).
* **Módulo de Histórico de Produção:** Consulta detalhada de apontamentos anteriores via `DataGridView` customizado em tema escuro, com mapeamento dinâmico de dados e paginação/listagem por operador.
* **Tratamento Avançado de Erros e JSON:** Conversores customizados (`FlexibleIntConverter`) para lidar com dados flexíveis vindos da API e captura amigável de exceções de negócio.
* **Interface Moderna (*Dark Mode*):** Customização programática de controles do Windows Forms para uma experiência visual limpa e ergonômica na fábrica/escritório.

---

## 📂 Arquitetura Completa da Solução

O ecossistema é dividido em projetos desacoplados seguindo os princípios de **Clean Architecture**, **DDD (Domain-Driven Design)** e **Componentização**:

```text
📦 ProductionOrdersSequor (Solução Global)
 ┣ 📜 ProductionOrdersSequor.sln
 ┃
 ┣ 📂 Backend (API e Camadas de Negócio)
 ┃ ┣ 📂 ProductionOrdersSequor.API          # Ponto de entrada ASP.NET Core Web API
 ┃ ┃ ┣ 📜 Program.cs                         # Configuração de DI, Middleware e Endpoints
 ┃ ┃ ┗ 📜 appsettings.json                   # Configurações de conexão e ambiente
 ┃ ┣ 📂 ProductionOrdersSequor.Application    # Regras de aplicação e casos de uso
 ┃ ┃ ┣ 📂 DTOs                               # Objetos de transferência de dados (Payload, Orders, History)
 ┃ ┃ ┣ 📂 Interfaces                         # Contratos de serviços da aplicação
 ┃ ┃ ┗ 📂 Services                           # Implementação das regras de negócio (OrderService, etc.)
 ┃ ┣ 📂 ProductionOrdersSequor.Domain         # Core da aplicação (Regras de Domínio)
 ┃ ┃ ┣ 📂 Entities                           # Entidades de Domínio (Production, Product, Material, etc.)
 ┃ ┃ ┗ 📂 Interfaces                         # Contratos de repositórios
 ┃ ┣ 📂 ProductionOrdersSequor.Infrastructure # Acesso a dados e persistência
 ┃ ┃ ┣ 📂 Context                            # DbContext do Entity Framework Core
 ┃ ┃ ┣ 📂 Mappings                           # Mapeamentos e configurações de ORM (Fluent API)
 ┃ ┃ ┗ 📂 Repositories                        # Implementação dos Repositórios (OrderRepository, etc.)
 ┃ ┗ 📂 ProductionOrdersSequor.Application.Tests # Suíte de testes unitários e de integração
 ┃
 ┣ 📂 Frontend Web (PRODUCTIONORDERSSEQUOR.UI)
 ┃ ┣ 📂 public/                              # Ativos públicos e favicon
 ┃ ┗ 📂 src/
 ┃   ┣ 📂 assets/                            # Imagens e estilos estáticos
 ┃   ┣ 📂 components/                        # Componentes reutilizáveis
 ┃   ┃ ┣ 📜 NotificationModal.jsx           # Modal de feedback de status/erros
 ┃   ┃ ┣ 📜 ProductionForm.jsx               # Formulário principal de apontamento
 ┃   ┃ ┗ 📜 ProductionHistory.jsx            # Tabela e listagem do histórico de produção
 ┃   ┣ 📂 hooks/                             # Hooks customizados React
 ┃   ┃ ┗ 📜 useProduction.js                 # Lógica do ciclo, temporizador e estado da ordem
 ┃   ┣ 📂 pages/                             # Páginas da aplicação
 ┃   ┃ ┗ 📂 Production/
 ┃   ┃   ┗ 📜 ProductionPage.jsx             # Página principal do operador
 ┃   ┣ 📂 services/                          # Comunicação HTTP
 ┃   ┃ ┗ 📜 api.js                           # Instância e chamadas Axios/Fetch para o Backend
 ┃   ┣ 📜 App.jsx                            # Componente raiz da aplicação
 ┃   ┣ 📜 main.jsx                           # Ponto de entrada do Vite/React
 ┃   ┗ 📜 package.json                       # Dependências e scripts do Node.js
 ┃
 ┗ 📂 Frontend Desktop (ProductionOrdersSequor.Desktop)
   ┣ 📂 Forms/                               # Janelas e formulários da interface
   ┃ ┗ 📜 HistoryForm.cs                     # Tela dedicada para consulta do histórico (Dark Mode)
   ┣ 📂 Models/                              # DTOs do cliente desktop (ProductionOrderDto, etc.)
   ┣ 📂 Services/                            # Serviços auxiliares e HTTP
   ┃ ┣ 📜 ApiClient.cs                       # Cliente HTTP assíncrono
   ┃ ┗ 📜 JsonConverters.cs                  # Conversores customizados (FlexibleIntConverter)
   ┗ 📜 MainForm.cs                          # Interface principal em WinForms com suporte a Dark Mode
```

## 🚀 Funcionalidades do Sistema
* Ciclo de Produção em Tempo Real: Cronômetro automatizado (TimerCycle / useProduction) que valida o tempo necessário do ciclo produtivo antes de liberar o botão de apontamento.
* Multiplataforma: Experiência consistente tanto via navegador (React / Vite) quanto em terminais operacionais industriais (Windows Forms .NET 8 com Dark Mode customizado
* Validação Inteligente e Dinâmica: Liberação de ações com base na validação simultânea de e-mail do operador, seleção da ordem, quantidade, ciclo e material utilizado.
* Histórico Detalhado: Exibição clara do histórico de produções com suporte ao mapeamento textual da descrição do material, ordenação por data/hora e filtro por e-mail do operador
* Resiliência de Comunicação HTTP/JSON: Tratamento de dados inconsistentes através de conversores flexíveis (FlexibleIntConverter) para garantir que variações na API não quebrem o cliente desktop.

## 🛠️ Tecnologias Utilizadas
* Linguagem: C# 12 / .NET 8
* Framework: ASP.NET Core Web API
* Persistência: Entity Framework Core (SQL Server / InMemory)
* Arquitetura: Clean Architecture + DDD + Repository Pattern

# Frontend Web
* Biblioteca UI: React 18
* Build Tool: Vite
* Linguagem: JavaScript (ES6+) / JSX
* Estilização: CSS3 puro e responsivo

# Frontend Desktop
* Tecnologia: .NET 8 Windows Forms
* Customização: Dark Mode nativo via código
* Comunicação: HttpClient e System.Text.Json

# ⚙️ Como Executar a Aplicação
# 1. Pré-requisitos
* .NET 8 SDK
* Node.js (Versão 18 ou superior)
* Visual Studio 2022 ou Visual Studio Code

2. Passo a Passo para Iniciar
🔹 1. Inicializar o Backend (API)

# Navegue até o diretório da API
```
cd ProductionOrdersSequor.API
```

# Restaure as dependências e execute
```
# Navegue até o diretório da API
cd ProductionOrdersSequor.API

# Restaure as dependências e execute
dotnet restore
dotnet run
```

# 🔹 2. Inicializar o Frontend Web (React)
```
# Navegue até a pasta da interface Web
cd PRODUCTIONORDERSSEQUOR.UI

# Instale as dependências
npm install

# Execute o servidor de desenvolvimento Vite
npm run dev
```

# 🔹 3. Inicializar o Cliente Desktop (WinForms)

* No Visual Studio, defina o projeto ProductionOrdersSequor.Desktop como projeto de inicialização. 
* Pressione F5 ou execute no terminal:
```
cd ProductionOrdersSequor.Desktop
dotnet run
```
