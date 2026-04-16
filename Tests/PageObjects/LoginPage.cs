using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Tests.PageObjects;

/// <summary>
/// Page Object da Página de Login da SumUp.
/// URL: https://me.sumup.com/pt-br/login
/// Encapsula o formulário de login, tratamento de captcha/MFA,
/// e alternativa com storage state para casos bloqueados.
/// </summary>
public class LoginPage : BasePage
{
    public LoginPage(IPage page) : base(page) { }

    // ===================== Navegação =====================

    /// <summary>Acessa a página de login da SumUp</summary>
    public async Task GoToAsync()
    {
        await NavigateToAsync(TestSettings.LoginUrl);
        await AcceptCookiesIfPresentAsync();
    }

    // ===================== Localizadores =====================

    /// <summary>Campo de email/usuário no formulário de login</summary>
    public ILocator CampoEmail => Page.Locator("input[type='email'], input[name='email'], input[id*='email'], input[placeholder*='email'], input[placeholder*='Email']").First;

    /// <summary>Campo de senha no formulário de login</summary>
    public ILocator CampoSenha => Page.Locator("input[type='password'], input[name='password'], input[id*='password']").First;

    /// <summary>Botão de submit do formulário de login</summary>
    public ILocator BotaoEntrar => Page.Locator("button[type='submit'], button:has-text('Entrar'), button:has-text('Login'), button:has-text('Log in'), button:has-text('Continuar'), button:has-text('Continue')").First;

    /// <summary>Link "Esqueci minha senha"</summary>
    public ILocator LinkEsqueciSenha => Page.Locator("a:has-text('Esqueceu'), a:has-text('Forgot'), a:has-text('esqueci'), a[href*='forgot'], a[href*='reset']").First;

    /// <summary>Mensagem de erro exibida após tentativa de login inválida</summary>
    public ILocator MensagemErro => Page.Locator("[class*='error'], [class*='alert'], [role='alert'], [class*='invalid'], p:has-text('incorrect'), p:has-text('inválid')").First;

    /// <summary>Elemento de captcha (reCAPTCHA, hCaptcha, etc.)</summary>
    public ILocator ElementoCaptcha => Page.Locator("iframe[src*='captcha'], iframe[src*='recaptcha'], iframe[src*='hcaptcha'], [class*='captcha']").First;

    /// <summary>Campo de MFA/2FA (código de verificação)</summary>
    public ILocator CampoMFA => Page.Locator("input[name*='code'], input[name*='otp'], input[name*='mfa'], input[placeholder*='código'], input[placeholder*='code']").First;

    // ===================== Ações =====================

    /// <summary>
    /// Realiza o fluxo completo de login com email e senha.
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <param name="senha">Senha do usuário</param>
    public async Task RealizarLoginAsync(string email, string senha)
    {
        await FillWithHighlightAsync(CampoEmail, email, "preencher-email-login");
        await FillWithHighlightAsync(CampoSenha, senha, "preencher-senha-login");
        await ClickWithHighlightAsync(BotaoEntrar, "clicar-botao-entrar");
    }

    /// <summary>
    /// Realiza login usando as credenciais configuradas no appsettings.json.
    /// </summary>
    public async Task RealizarLoginComCredenciaisConfiguradasAsync()
    {
        await RealizarLoginAsync(TestSettings.Email, TestSettings.Password);
    }

    /// <summary>
    /// Realiza login com credenciais inválidas para teste de caso negativo.
    /// </summary>
    public async Task RealizarLoginInvalidoAsync()
    {
        await RealizarLoginAsync("email_invalido@teste.com", "SenhaErrada123!");
    }

    /// <summary>Clica no link "Esqueci minha senha"</summary>
    public async Task ClicarEsqueciSenhaAsync()
    {
        await ClickWithHighlightAsync(LinkEsqueciSenha, "clicar-esqueci-senha");
    }

    // ===================== Validações =====================

    /// <summary>Verifica se a página de login carregou</summary>
    public async Task<bool> IsPaginaCarregadaAsync()
    {
        return await IsElementVisibleAsync(CampoEmail) || await IsElementVisibleAsync(BotaoEntrar);
    }

    /// <summary>Verifica se o formulário de login está visível</summary>
    public async Task<bool> IsFormularioVisivelAsync()
    {
        return await IsElementVisibleAsync(CampoEmail);
    }

    /// <summary>Verifica se uma mensagem de erro está sendo exibida</summary>
    public async Task<bool> IsMensagemErroVisivelAsync()
    {
        try
        {
            return await MensagemErro.IsVisibleAsync();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Retorna o texto da mensagem de erro</summary>
    public async Task<string> GetMensagemErroAsync()
    {
        try
        {
            return await MensagemErro.TextContentAsync() ?? "";
        }
        catch
        {
            return "";
        }
    }

    /// <summary>Verifica se há captcha bloqueando o login</summary>
    public async Task<bool> HasCaptchaAsync()
    {
        return await IsElementVisibleAsync(ElementoCaptcha);
    }

    /// <summary>Verifica se a tela de MFA/2FA foi exibida</summary>
    public async Task<bool> HasMFAAsync()
    {
        return await IsElementVisibleAsync(CampoMFA);
    }

    // ===================== Storage State (Alternativa para bloqueios) =====================

    /// <summary>
    /// Salva o estado de autenticação (cookies e localStorage) em um arquivo.
    /// Útil para restaurar a sessão sem refazer login em testes subsequentes.
    /// </summary>
    /// <param name="filePath">Caminho do arquivo de storage state</param>
    public async Task SalvarStorageStateAsync(string filePath = "Config/storageState.json")
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", filePath);
        await Page.Context.StorageStateAsync(new BrowserContextStorageStateOptions
        {
            Path = fullPath
        });
        Console.WriteLine($"[AUTH] Storage state salvo em: {fullPath}");
    }
}
