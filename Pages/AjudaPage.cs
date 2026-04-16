using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Pages;

/// <summary>
/// Page Object da Página de Ajuda/Suporte da SumUp.
/// URL: https://www.sumup.com/pt-br/ajuda/
/// Encapsula campo de busca, categorias de ajuda e artigos de suporte.
/// </summary>
public class AjudaPage : BasePage
{
    public AjudaPage(IPage page) : base(page) { }

    // ===================== Navegação =====================

    /// <summary>Acessa a página de ajuda da SumUp</summary>
    public async Task GoToAsync()
    {
        await NavigateToAsync(TestSettings.AjudaUrl);
        await AcceptCookiesIfPresentAsync();
    }

    // ===================== Localizadores =====================

    /// <summary>Título principal da página de ajuda</summary>
    public ILocator TituloPagina => Page.Locator("h1").First;

    /// <summary>Campo de busca do centro de ajuda</summary>
    public ILocator CampoBusca => Page.Locator("input[type='search'], input[type='text'][placeholder*='busca'], input[placeholder*='search'], input[placeholder*='pesquis']").First;

    /// <summary>Categorias/tópicos de ajuda</summary>
    public ILocator CategoriaAjuda => Page.Locator("[class*='category'], [class*='topic'], [class*='card'] a, main a[href*='ajuda']");

    /// <summary>Links para artigos de ajuda</summary>
    public ILocator LinksArtigos => Page.Locator("main a[href*='ajuda'], main a[href*='help'], main a[href*='support']");

    /// <summary>Botão de contato/suporte</summary>
    public ILocator BotaoContato => Page.Locator("a:has-text('Contato'), a:has-text('Fale'), a:has-text('Contact'), button:has-text('Contato')").First;

    // ===================== Ações =====================

    /// <summary>Realiza uma busca no centro de ajuda</summary>
    /// <param name="termo">Termo de busca</param>
    public async Task BuscarAsync(string termo)
    {
        await FillWithHighlightAsync(CampoBusca, termo, "preencher-busca-ajuda");
        await Page.Keyboard.PressAsync("Enter");
        await Task.Delay(1000); // Aguarda resultados
    }

    /// <summary>Clica na primeira categoria de ajuda disponível</summary>
    public async Task ClicarPrimeiraCategoriaAsync()
    {
        await ClickWithHighlightAsync(CategoriaAjuda.First, "clicar-primeira-categoria");
    }

    // ===================== Validações =====================

    /// <summary>Verifica se a página de ajuda carregou corretamente</summary>
    public async Task<bool> IsPaginaCarregadaAsync()
    {
        return await IsElementVisibleAsync(TituloPagina);
    }

    /// <summary>Verifica se o campo de busca está disponível</summary>
    public async Task<bool> IsCampoBuscaVisivelAsync()
    {
        return await IsElementVisibleAsync(CampoBusca);
    }

    /// <summary>Retorna o número de categorias de ajuda visíveis</summary>
    public async Task<int> GetQuantidadeCategoriasAsync()
    {
        return await CategoriaAjuda.CountAsync();
    }

    /// <summary>Retorna o texto do título da página</summary>
    public async Task<string> GetTituloPaginaAsync()
    {
        try
        {
            return await TituloPagina.TextContentAsync() ?? "";
        }
        catch
        {
            return "";
        }
    }
}
