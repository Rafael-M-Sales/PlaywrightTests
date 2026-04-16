using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Tests.PageObjects;

/// <summary>
/// Page Object da Página de Produtos/Maquininhas da SumUp.
/// URL: https://www.sumup.com/pt-br/maquininhas-de-cartao/
/// Encapsula a listagem de produtos, cards de maquininhas e botões de compra.
/// </summary>
public class ProdutosPage : BasePage
{
    public ProdutosPage(IPage page) : base(page) { }

    // ===================== Navegação =====================

    /// <summary>Acessa a página de produtos (maquininhas)</summary>
    public async Task GoToAsync()
    {
        await NavigateToAsync(TestSettings.ProdutosUrl);
        await AcceptCookiesIfPresentAsync();
    }

    // ===================== Localizadores =====================

    /// <summary>Título principal da página de produtos</summary>
    public ILocator TituloPagina => Page.Locator("h1").First;

    /// <summary>Cards de produtos exibidos na página</summary>
    public ILocator CardsProdutos => Page.Locator("[class*='card'], [class*='product'], article");

    /// <summary>Botões de compra/CTA dos produtos</summary>
    public ILocator BotoesComprar => Page.Locator("a:has-text('Comprar'), a:has-text('Peça'), button:has-text('Comprar'), button:has-text('Peça')");

    /// <summary>Seção de preços/valores dos produtos</summary>
    public ILocator SecaoPrecos => Page.Locator("[class*='price'], [class*='preco'], [class*='valor']");

    /// <summary>Imagens dos produtos</summary>
    public ILocator ImagensProdutos => Page.Locator("main img[src*='product'], main img[alt*='maquininha'], main img[alt*='SumUp']");

    // ===================== Ações =====================

    /// <summary>Clica no botão de compra do primeiro produto listado</summary>
    public async Task ClicarComprarPrimeiroProdutoAsync()
    {
        await ClickWithHighlightAsync(BotoesComprar.First, "clicar-comprar-primeiro-produto");
    }

    // ===================== Validações =====================

    /// <summary>Verifica se a página de produtos carregou corretamente</summary>
    public async Task<bool> IsPaginaCarregadaAsync()
    {
        return await IsElementVisibleAsync(TituloPagina);
    }

    /// <summary>Retorna a quantidade de cards/produtos exibidos na página</summary>
    public async Task<int> GetQuantidadeProdutosAsync()
    {
        return await CardsProdutos.CountAsync();
    }

    /// <summary>Verifica se existem preços visíveis na página</summary>
    public async Task<bool> HasPrecosVisiveis()
    {
        return (await SecaoPrecos.CountAsync()) > 0;
    }

    /// <summary>Retorna o texto do título principal</summary>
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
