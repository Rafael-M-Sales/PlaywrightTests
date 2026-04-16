using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightTests.Pages;
using Reqnroll;

namespace PlaywrightTests.Steps;

[Binding]
public class HomePageSteps
{
    private readonly HomePage _homePage;
    private readonly ScenarioContext _scenarioContext;

    public HomePageSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        var page = _scenarioContext.Get<IPage>("Page");
        _homePage = new HomePage(page);
        _homePage.SetScenario(_scenarioContext.ScenarioInfo.Title);
    }

    [Given(@"que eu acesso a página inicial da SumUp")]
    public async Task DadoQueEuAcessoAPaginaInicialDaSumUp()
    {
        await _homePage.GoToAsync();
    }

    [Then(@"a página inicial deve carregar completamente")]
    public async Task EntaoAPaginaInicialDeveCarregarCompletamente()
    {
        bool carregada = await _homePage.IsPaginaCarregadaAsync();
        Assert.That(carregada, Is.True, "A página inicial não carregou os elementos principais.");
    }

    [Then(@"o logo da SumUp deve estar visível")]
    public async Task EntaoOLogoDaSumUpDeveEstarVisivel()
    {
        bool visivel = await _homePage.IsLogoVisibleAsync();
        Assert.That(visivel, Is.True, "O logo da SumUp não está visível no header.");
    }

    [Then(@"o header deve estar visível")]
    public async Task EntaoOHeaderDeveEstarVisivel()
    {
        bool visivel = await _homePage.IsHeaderVisibleAsync();
        Assert.That(visivel, Is.True, "O header principal não está visível.");
    }

    [Then(@"o footer deve estar visível")]
    public async Task EntaoOFooterDeveEstarVisivel()
    {
        bool visivel = await _homePage.IsFooterVisibleAsync();
        Assert.That(visivel, Is.True, "O footer não está visível.");
    }

    [Then(@"o título da página deve conter ""(.*)""")]
    public async Task EntaoOTituloDaPaginaDeveConter(string textoEsperado)
    {
        string titulo = await _homePage.GetPageTitleAsync();
        Assert.That(titulo, Does.Contain(textoEsperado), $"O título da página '{titulo}' não contém '{textoEsperado}'.");
    }

    [Then(@"o menu de navegação deve estar visível")]
    public async Task EntaoOMenuDeNavegacaoDeveEstarVisivel()
    {
        bool visivel = await _homePage.IsMenuVisivelAsync();
        Assert.That(visivel, Is.True, "O menu de navegação não está visível.");
    }

    [Then(@"o botão CTA principal deve estar visível")]
    public async Task EntaoOBotaoCTAPrincipalDeveEstarVisivel()
    {
        bool visivel = await _homePage.BotaoCTAPrincipal.IsVisibleAsync();
        Assert.That(visivel, Is.True, "O botão CTA principal não está visível.");
    }

    [Then(@"o título principal deve estar visível")]
    public async Task EntaoOTituloPrincipalDeveEstarVisivel()
    {
        bool visivel = await _homePage.TituloPrincipal.IsVisibleAsync();
        Assert.That(visivel, Is.True, "O título principal (H1) não está visível.");
    }
}
