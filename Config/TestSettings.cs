using Microsoft.Extensions.Configuration;

namespace PlaywrightTests.Config;

/// <summary>
/// Classe responsável por carregar e fornecer as configurações do projeto.
/// Suporta múltiplos ambientes (dev, homologação, produção) via appsettings.
/// A seleção do ambiente pode ser feita via variável de ambiente TEST_ENVIRONMENT.
/// </summary>
public static class TestSettings
{
    private static IConfiguration? _configuration;

    /// <summary>
    /// Carrega a configuração do ambiente correto com base na variável TEST_ENVIRONMENT.
    /// Prioridade: variáveis de ambiente > appsettings.{ambiente}.json > appsettings.json
    /// </summary>
    public static IConfiguration Configuration
    {
        get
        {
            if (_configuration == null)
            {
                var environment = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? "producao";

                _configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Config"))
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            }
            return _configuration;
        }
    }

    // ===================== URLs =====================

    /// <summary>URL base da aplicação SumUp</summary>
    public static string BaseUrl => Configuration["TestSettings:BaseUrl"] ?? "https://www.sumup.com/pt-br/";

    /// <summary>URL da página de login</summary>
    public static string LoginUrl => Configuration["TestSettings:LoginUrl"] ?? "https://me.sumup.com/pt-br/login";

    /// <summary>URL da página de produtos (maquininhas)</summary>
    public static string ProdutosUrl => Configuration["TestSettings:ProdutosUrl"] ?? "https://www.sumup.com/pt-br/maquininhas-de-cartao/";

    /// <summary>URL da página de taxas</summary>
    public static string TaxasUrl => Configuration["TestSettings:TaxasUrl"] ?? "https://www.sumup.com/pt-br/taxas/";

    /// <summary>URL da página de ajuda</summary>
    public static string AjudaUrl => Configuration["TestSettings:AjudaUrl"] ?? "https://www.sumup.com/pt-br/ajuda/";

    // ===================== Browser =====================

    /// <summary>Navegador a ser utilizado (chromium, firefox, webkit)</summary>
    public static string Browser => Configuration["TestSettings:Browser"] ?? "chromium";

    /// <summary>Execução em modo headless (sem interface gráfica)</summary>
    public static bool Headless => bool.Parse(Configuration["TestSettings:Headless"] ?? "true");

    /// <summary>Delay em milissegundos entre ações (útil para debug)</summary>
    public static int SlowMo => int.Parse(Configuration["TestSettings:SlowMo"] ?? "0");

    /// <summary>Timeout padrão em milissegundos para operações do Playwright</summary>
    public static int Timeout => int.Parse(Configuration["TestSettings:Timeout"] ?? "30000");

    // ===================== Evidências =====================

    /// <summary>Capturar screenshot automaticamente em caso de falha</summary>
    public static bool ScreenshotOnFailure => bool.Parse(Configuration["TestSettings:ScreenshotOnFailure"] ?? "true");

    /// <summary>Gravar vídeo da execução dos testes</summary>
    public static bool VideoRecording => bool.Parse(Configuration["TestSettings:VideoRecording"] ?? "true");

    /// <summary>Aplicar contorno vermelho (highlight) nos elementos interagidos</summary>
    public static bool HighlightElements => bool.Parse(Configuration["TestSettings:HighlightElements"] ?? "true");

    /// <summary>Caminho base para salvar evidências</summary>
    public static string EvidenciasPath => Configuration["TestSettings:EvidenciasPath"] ?? "Evidencias";

    /// <summary>Nome do ambiente atual</summary>
    public static string Environment_ => Configuration["TestSettings:Environment"] ?? "producao";

    // ===================== Credenciais =====================

    /// <summary>Email para login (pode ser sobrescrito via variável de ambiente)</summary>
    public static string Email => Configuration["Credentials:Email"] ?? "";

    /// <summary>Senha para login (pode ser sobrescrita via variável de ambiente)</summary>
    public static string Password => Configuration["Credentials:Password"] ?? "";
}
