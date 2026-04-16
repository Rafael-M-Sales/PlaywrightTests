using Microsoft.Playwright;
using PlaywrightTests.Config;
using PlaywrightTests.Utils;
using Reqnroll;
using Allure.Net.Commons;
using Microsoft.VisualBasic;

namespace PlaywrightTests.Hooks;

[Binding]
public class TestHooks
{
    private readonly ScenarioContext _scenarioContext;
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    public TestHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        _playwright = await Playwright.CreateAsync();
        
        var browserType = TestSettings.Browser.ToLower() switch
        {
            "firefox" => _playwright.Firefox,
            "webkit" => _playwright.Webkit,
            _ => _playwright.Chromium
        };

        _browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = TestSettings.Headless,
            SlowMo = TestSettings.SlowMo
        });

        var contextOptions = new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            RecordVideoDir = TestSettings.VideoRecording ? EvidenceHelper.GetEvidencePath(_scenarioContext.ScenarioInfo.Title) : null
        };

        _context = await _browser.NewContextAsync(contextOptions);
        _page = await _context.NewPageAsync();

        // Compartilha a página no contexto do cenário para as Step Definitions
        _scenarioContext["Page"] = _page;
        _scenarioContext["StepCount"] = 0;
    }

    [AfterStep]
    public async Task AfterStep()
    {
        var page = _scenarioContext.Get<IPage>("Page");
        var stepCount = _scenarioContext.Get<int>("StepCount") + 1;
        _scenarioContext["StepCount"] = stepCount;

        var stepName = _scenarioContext.StepContext.StepInfo.Text;
        
        // Captura screenshot de cada step
        var screenshotPath = await EvidenceHelper.CaptureScreenshotAsync(
            page, 
            _scenarioContext.ScenarioInfo.Title, 
            stepName, 
            stepCount
        );

        // Anexa ao Allure
        AllureApi.AddAttachment(stepName, "image/png", screenshotPath);
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        if (_scenarioContext.TestError != null)
        {
            var page = _scenarioContext.Get<IPage>("Page");
            var failurePath = await EvidenceHelper.CaptureFailureScreenshotAsync(
                page, 
                _scenarioContext.ScenarioInfo.Title, 
                _scenarioContext.TestError.Message
            );
            AllureApi.AddAttachment("FALHA_DETALHADA", "image/png", failurePath);
        }

        if (_page != null) await _page.CloseAsync();
        if (_context != null) await _context.CloseAsync();
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
        
        Console.WriteLine($"[INFO] Finalizado cenário: {_scenarioContext.ScenarioInfo.Title}");
    }
}
