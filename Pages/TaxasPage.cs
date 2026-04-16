using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Pages;

/// <summary>
/// Page Object da Página de Taxas da SumUp.
/// URL: https://www.sumup.com/pt-br/taxas/
/// Encapsula a visualização de taxas, tabelas de preços e comparações de planos.
/// </summary>
public class TaxasPage : BasePage
{
    public TaxasPage(IPage page) : base(page) { }

    // ===================== Navegação =====================

    /// <summary>Acessa a página de taxas</summary>
    public async Task GoToAsync()
    {
        await NavigateToAsync(TestSettings.TaxasUrl);
        await AcceptCookiesIfPresentAsync();
    }

    // ===================== Localizadores =====================

    /// <summary>Título principal da página de taxas</summary>
    public ILocator TituloPagina => Page.Locator("h1").First;

    /// <summary>Informações de taxas/porcentagens exibidas</summary>
    public ILocator InfoTaxas => Page.Locator("[class*='rate'], [class*='taxa'], [class*='fee'], [class*='percent']");

    /// <summary>Tabelas de comparação de planos</summary>
    public ILocator TabelaPlanos => Page.Locator("table, [class*='table'], [class*='plan'], [class*='pricing']");

    /// <summary>Botões de CTA na página de taxas</summary>
    public ILocator BotoesCTA => Page.Locator("a:has-text('Comprar'), a:has-text('Peça'), a:has-text('Começar'), button:has-text('Comprar')");

    /// <summary>Seções de FAQ ou perguntas frequentes</summary>
    public ILocator SecaoFAQ => Page.Locator("[class*='faq'], [class*='accordion'], details, summary");

    // ===================== Validações =====================

    /// <summary>Verifica se a página de taxas carregou corretamente</summary>
    public async Task<bool> IsPaginaCarregadaAsync()
    {
        return await IsElementVisibleAsync(TituloPagina);
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

    /// <summary>Verifica se informações de taxas estão presentes</summary>
    public async Task<bool> HasInformacoesTaxasAsync()
    {
        // Verifica se há algum conteúdo relacionado a taxas na página
        var pageContent = await Page.ContentAsync();
        return pageContent.Contains("%") || pageContent.Contains("taxa", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Verifica se existe seção de FAQ na página</summary>
    public async Task<bool> HasFAQAsync()
    {
        return (await SecaoFAQ.CountAsync()) > 0;
    }
}
