using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightTests.Config;

namespace PlaywrightTests.Tests.Api;

/// <summary>
/// Exemplo de Testes de API utilizando o Playwright APIRequestContext.
/// Valida endpoints REST da SumUp para garantir integridade dos dados.
/// </summary>
[TestFixture]
public class SumUpApiTests
{
    private IPlaywright? _playwright;
    private IAPIRequestContext? _requestContext;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _requestContext = await _playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = "https://api.sumup.com", // Exemplo de URL de API
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                { "Accept", "application/json" }
            }
        });
    }

    [Test]
    [Category("API")]
    public async Task ValidarEndpointPublicoAsync()
    {
        // Exemplo: Consultar informações públicas ou status da API
        var response = await _requestContext!.GetAsync("/v0.1/me");

        Console.WriteLine($"[API] Status Code: {response.Status}");
        Assert.That(response.Status, Is.EqualTo(401).Or.EqualTo(403), "O endpoint deveria exigir autenticação.");
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        if (_requestContext != null) await _requestContext.DisposeAsync();
        _playwright?.Dispose();
    }
}
