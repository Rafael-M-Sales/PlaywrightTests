using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightTests.Config;
using PlaywrightTests.Utils;

namespace PlaywrightTests.Tests.Visual;

/// <summary>
/// Testes Visuais para garantir que a interface da SumUp não sofre regressões estéticas.
/// Utiliza captura de snapshots para comparação manual ou via ferramentas de diff.
/// </summary>
[TestFixture]
public class SumUpVisualTests
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    [SetUp]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
    }

    [Test]
    [Category("Visual")]
    public async Task ValidarVisualHomepageAsync()
    {
        var context = await _browser!.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
        });
        var page = await context.NewPageAsync();

        await page.GotoAsync(TestSettings.BaseUrl);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Captura o baseline visual
        string baseline = await ScreenshotHelper.CaptureVisualBaselineAsync(page, "Homepage_Desktop_1280x720");
        
        Console.WriteLine($"[VISUAL] Snapshot capturado em: {baseline}");
        Assert.That(File.Exists(baseline), Is.True, "O snapshot visual não foi gerado.");
    }

    [Test]
    [Category("Visual")]
    public async Task ValidarVisualHeaderAsync()
    {
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync(TestSettings.BaseUrl);

        var header = page.Locator("header").First;
        string headerSnap = Path.Combine(AppContext.BaseDirectory, "header_visual.png");
        
        await ScreenshotHelper.CaptureElementScreenshotAsync(header, headerSnap);
        
        Assert.That(File.Exists(headerSnap), Is.True, "O snapshot visual do header não foi gerado.");
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }
}
