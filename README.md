# 💳 SumUp Automation Suite (Playwright + C#)

Projeto de automação profissional de ponta a ponta (E2E) para a aplicação web da SumUp.

---

## 🛠 Tecnologias e Arquitetura

- **Linguagem:** C# (.NET 8)
- **Framework de Automação:** [Playwright](https://playwright.dev/dotnet/)
- **BDD:** [Reqnroll](https://reqnroll.net/) (Sucessor do SpecFlow para .NET 8)
- **Padrão de Projeto:** Page Object Model (POM)
- **Relatórios:** Allure Report
- **Infraestrutura:** Docker, Kubernetes (EKS), Jenkins, AWS

---

## 📂 Estrutura do Projeto

- **Features/**: Arquivos Gherkin (.feature) em português.
- **Steps/**: Implementação técnica dos passos do Gherkin.
- **Pages/**: Page Objects com seletores e métodos de interação.
- **Hooks/**: Gerenciamento do ciclo de vida (Setup/Teardown) e evidências.
- **Utils/**: Helpers para Screenshots, Highlight e Evidências.
- **Config/**: Gerenciamento de múltiplos ambientes (dev, homolog, prod).
- **Tests/**: Suítes específicas para API, Performance e Visual.

---

## 🚀 Como Rodar Localmente

### Pré-requisitos
1. [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. [PowerShell](https://github.com/PowerShell/PowerShell) (para scripts do Playwright)

### Passo a Passo

1. **Instalar dependências e Buildar:**
   ```powershell
   dotnet build
   ```

2. **Instalar Navegadores do Playwright:**
   ```powershell
   pwsh bin/Debug/net8.0/playwright.ps1 install
   ```

3. **Executar todos os testes:**
   ```powershell
   dotnet test
   ```

4. **Executar por categoria (ex: Smoke):**
   ```powershell
   dotnet test --filter "Category=smoke"
   ```

---

## 🌍 Gerenciamento de Ambientes

O projeto suporta múltiplos ambientes via variável de ambiente `TEST_ENVIRONMENT`. 
Valores aceitos: `dev`, `homologacao`, `producao` (default).

```powershell
$env:TEST_ENVIRONMENT="homologacao"; dotnet test
```

---

## 📊 Relatórios e Evidências

- **Screenshots:** Gerados automaticamente para cada passo do Gherkin na pasta `/Evidencias`.
- **Vídeos:** Gravados por cenário em caso de falha ou conforme `appsettings.json`.
- **Allure Report:**
  Para visualizar o relatório após os testes:
  ```powershell
  allure serve allure-results
  ```

---

## 🚢 CI/CD e Containerização

O projeto está pronto para esteira Jenkins utilizando o `Infrastructure/Jenkinsfile`.

**Docker:**
```bash
docker build -t sumup-tests -f Infrastructure/Dockerfile .
docker run sumup-tests
```

**Kubernetes:**
Os manifests em `Infrastructure/kubernetes/k8s.yaml` permitem a execução escalável em clusters AWS EKS.

---

## ✍️ Autor
**Rafael M. Sales**  
GitHub: [Rafael-M-Sales](https://github.com/Rafael-M-Sales)
