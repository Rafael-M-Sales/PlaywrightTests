using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightTests.Config;

namespace PlaywrightTests.Tests.Specs;

/// <summary>
/// Testes de Performance básica utilizando métricas do navegador (Performance Timeline API).
/// Valida tempos de resposta e carregamento da aplicação SumUp.
/// </summary>
[TestFixture]
public class SumUpPerformanceTests
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
    [Category("Performance")]
    public async Task ValidarTempoCarregamentoHomepageAsync()
    {
        var context = await _browser!.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync(TestSettings.BaseUrl);

        // Captura métricas de navegação via JavaScript
        var performanceMetrics = await page.EvaluateAsync<Dictionary<string, double>>(@"() => {
            const timing = window.performance.timing;
            return {
                'loadTime': timing.loadEventEnd - timing.navigationStart,
                'domReady': timing.domComplete - timing.responseEnd,
                'ttfb': timing.responseStart - timing.navigationStart
            };
        }");

        double loadTimeSec = performanceMetrics["loadTime"] / 1000;
        Console.WriteLine($"[PERFORMANCE] Homepage Load Time: {loadTimeSec}s");

        // Asserção: Homepage deve carregar em menos de 5 segundos
        Assert.That(loadTimeSec, Is.LessThan(5.0), "A homepage está demorando demais para carregar.");
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }
}
