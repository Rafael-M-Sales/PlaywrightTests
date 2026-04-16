using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Pages;

/// <summary>
/// Page Object da Página Inicial da SumUp (https://www.sumup.com/pt-br/).
/// Encapsula todos os elementos e ações disponíveis na homepage:
/// - Menu de navegação principal
/// - Botões de CTA (Call to Action)
/// - Seções de conteúdo (produtos, benefícios)
/// - Links para outras áreas do site
/// </summary>
public class HomePage : BasePage
{
    public HomePage(IPage page) : base(page) { }

    // ===================== Navegação =====================

    /// <summary>
    /// Acessa a página inicial da SumUp.
    /// </summary>
    public async Task GoToAsync()
    {
        await NavigateToAsync(TestSettings.BaseUrl);
        await AcceptCookiesIfPresentAsync();
    }

    // ===================== Localizadores — Menu Principal =====================

    /// <summary>Menu de navegação principal do header</summary>
    public ILocator MenuNavegacao => Page.Locator("nav, header nav").First;

    /// <summary>Link "Produtos" no menu de navegação</summary>
    public ILocator LinkProdutos => Page.Locator("a:has-text('Produtos'), a:has-text('Maquininhas'), nav a[href*='maquininhas'], nav a[href*='produtos']").First;

    /// <summary>Link "Taxas" no menu de navegação</summary>
    public ILocator LinkTaxas => Page.Locator("a:has-text('Taxas'), nav a[href*='taxas']").First;

    /// <summary>Link "Ajuda" no menu de navegação</summary>
    public ILocator LinkAjuda => Page.Locator("a:has-text('Ajuda'), a:has-text('Suporte'), nav a[href*='ajuda']").First;

    /// <summary>Botão/Link de Login no header</summary>
    public ILocator BotaoLogin => Page.Locator("a:has-text('Login'), a:has-text('Entrar'), a[href*='login']").First;

    // ===================== Localizadores — Conteúdo Principal =====================

    /// <summary>Título principal (hero) da homepage</summary>
    public ILocator TituloPrincipal => Page.Locator("h1").First;

    /// <summary>Botão CTA principal (ex: "Peça sua SumUp", "Comprar agora")</summary>
    public ILocator BotaoCTAPrincipal => Page.Locator("main a[class*='button'], main button, a:has-text('Peça'), a:has-text('Comprar'), a:has-text('Começar')").First;

    // ===================== Ações =====================

    /// <summary>Clica no link de Produtos no menu de navegação</summary>
    public async Task ClicarProdutosAsync()
    {
        await ClickWithHighlightAsync(LinkProdutos, "clicar-link-produtos");
    }

    /// <summary>Clica no link de Taxas no menu de navegação</summary>
    public async Task ClicarTaxasAsync()
    {
        await ClickWithHighlightAsync(LinkTaxas, "clicar-link-taxas");
    }

    /// <summary>Clica no link de Ajuda no menu de navegação</summary>
    public async Task ClicarAjudaAsync()
    {
        await ClickWithHighlightAsync(LinkAjuda, "clicar-link-ajuda");
    }

    /// <summary>Clica no botão de Login</summary>
    public async Task ClicarLoginAsync()
    {
        await ClickWithHighlightAsync(BotaoLogin, "clicar-botao-login");
    }

    /// <summary>Clica no botão CTA principal da homepage</summary>
    public async Task ClicarCTAPrincipalAsync()
    {
        await ClickWithHighlightAsync(BotaoCTAPrincipal, "clicar-cta-principal");
    }

    // ===================== Validações =====================

    /// <summary>Verifica se a página inicial carregou completamente</summary>
    public async Task<bool> IsPaginaCarregadaAsync()
    {
        var headerVisible = await IsHeaderVisibleAsync();
        var titleVisible = await IsElementVisibleAsync(TituloPrincipal);
        return headerVisible && titleVisible;
    }

    /// <summary>Verifica se o menu de navegação está visível</summary>
    public async Task<bool> IsMenuVisivelAsync()
    {
        return await IsElementVisibleAsync(MenuNavegacao);
    }

    /// <summary>Verifica se o título principal contém texto esperado</summary>
    public async Task<string> GetTituloPrincipalAsync()
    {
        try
        {
            return await TituloPrincipal.TextContentAsync() ?? "";
        }
        catch
        {
            return "";
        }
    }
}
