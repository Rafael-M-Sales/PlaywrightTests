using Microsoft.Playwright;
using PlaywrightTests.Config;
using PlaywrightTests.Utils;

namespace PlaywrightTests.Pages;

/// <summary>
/// Classe base para todos os Page Objects do projeto.
/// Implementa o padrão Page Object Model (POM) centralizado com:
/// - Navegação base
/// - Highlight automático em elementos interagidos
/// - Captura de screenshots por step
/// - Métodos comuns a todas as páginas (header, footer, menu)
/// </summary>
public abstract class BasePage
{
    protected readonly IPage Page;
    private int _stepCounter = 0;
    private string _currentScenario = "default";

    /// <summary>
    /// Construtor base — recebe a instância de IPage do Playwright.
    /// </summary>
    protected BasePage(IPage page)
    {
        Page = page;
    }

    /// <summary>
    /// Define o nome do cenário atual para organização das evidências.
    /// </summary>
    public void SetScenario(string scenarioName)
    {
        _currentScenario = scenarioName;
        _stepCounter = 0;
    }

    // ===================== Navegação =====================

    /// <summary>
    /// Navega para uma URL específica e aguarda o carregamento completo (networkidle).
    /// </summary>
    protected async Task NavigateToAsync(string url)
    {
        await Page.GotoAsync(url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = TestSettings.Timeout
        });
    }

    /// <summary>
    /// Retorna o título da página atual.
    /// </summary>
    public async Task<string> GetPageTitleAsync()
    {
        return await Page.TitleAsync();
    }

    /// <summary>
    /// Retorna a URL atual da página.
    /// </summary>
    public string GetCurrentUrl()
    {
        return Page.Url;
    }

    /// <summary>
    /// Aguarda o carregamento completo da página (estado networkidle).
    /// </summary>
    public async Task WaitForPageLoadAsync()
    {
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    // ===================== Interação com Highlight =====================

    /// <summary>
    /// Clica em um elemento aplicando highlight vermelho antes da ação.
    /// Captura screenshot automaticamente como evidência do step.
    /// </summary>
    protected async Task ClickWithHighlightAsync(ILocator locator, string stepDescription = "")
    {
        if (TestSettings.HighlightElements)
        {
            await HighlightHelper.HighlightElementAsync(Page, locator);
        }

        await CaptureStepScreenshotAsync(stepDescription);
        await locator.ClickAsync();
    }

    /// <summary>
    /// Preenche um campo de texto aplicando highlight e capturando evidência.
    /// </summary>
    protected async Task FillWithHighlightAsync(ILocator locator, string value, string stepDescription = "")
    {
        if (TestSettings.HighlightElements)
        {
            await HighlightHelper.HighlightElementAsync(Page, locator);
        }

        await CaptureStepScreenshotAsync(stepDescription);
        await locator.FillAsync(value);
    }

    /// <summary>
    /// Verifica se um elemento está visível na página.
    /// </summary>
    protected async Task<bool> IsElementVisibleAsync(ILocator locator)
    {
        try
        {
            return await locator.IsVisibleAsync();
        }
        catch
        {
            return false;
        }
    }

    // ===================== Evidências =====================

    /// <summary>
    /// Captura screenshot do step atual, incrementando o contador automaticamente.
    /// </summary>
    protected async Task CaptureStepScreenshotAsync(string stepDescription)
    {
        _stepCounter++;
        var description = string.IsNullOrEmpty(stepDescription) ? $"step_{_stepCounter}" : stepDescription;

        try
        {
            await EvidenceHelper.CaptureScreenshotAsync(
                Page, _currentScenario, description, _stepCounter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AVISO] Não foi possível capturar screenshot: {ex.Message}");
        }
    }

    // ===================== Elementos Comuns (Header/Footer) =====================

    /// <summary>Localizador do logo SumUp no header</summary>
    public ILocator LogoSumUp => Page.Locator("header a[href*='sumup.com'] img, header svg, header a[aria-label*='SumUp']").First;

    /// <summary>Verifica se o logo da SumUp está visível no header</summary>
    public async Task<bool> IsLogoVisibleAsync()
    {
        return await IsElementVisibleAsync(LogoSumUp);
    }

    /// <summary>Verifica se o header principal está carregado</summary>
    public async Task<bool> IsHeaderVisibleAsync()
    {
        return await IsElementVisibleAsync(Page.Locator("header").First);
    }

    /// <summary>Verifica se o footer da página está carregado</summary>
    public async Task<bool> IsFooterVisibleAsync()
    {
        return await IsElementVisibleAsync(Page.Locator("footer").First);
    }

    /// <summary>
    /// Aceita cookies se o banner de consentimento estiver visível.
    /// Necessário para evitar que o banner sobreponha elementos durante os testes.
    /// </summary>
    public async Task AcceptCookiesIfPresentAsync()
    {
        try
        {
            var cookieButton = Page.Locator("button:has-text('Aceitar'), button:has-text('Accept'), button[id*='cookie'], button[class*='cookie']").First;
            if (await cookieButton.IsVisibleAsync())
            {
                await cookieButton.ClickAsync();
                await Task.Delay(500);
            }
        }
        catch
        {
            // Banner de cookies não presente — segue normalmente
        }
    }
}
