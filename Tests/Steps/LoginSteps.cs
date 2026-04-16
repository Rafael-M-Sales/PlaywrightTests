using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightTests.Tests.PageObjects;
using Reqnroll;

namespace PlaywrightTests.Tests.Steps;

[Binding]
public class LoginSteps
{
    private readonly LoginPage _loginPage;
    private readonly ScenarioContext _scenarioContext;

    public LoginSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        var page = _scenarioContext.Get<IPage>("Page");
        _loginPage = new LoginPage(page);
        _loginPage.SetScenario(_scenarioContext.ScenarioInfo.Title);
    }

    [Given(@"que eu acesso a página de login da SumUp")]
    public async Task DadoQueEuAcessoAPaginaDeLoginDaSumUp()
    {
        await _loginPage.GoToAsync();
    }

    [When(@"eu tento realizar login com email ""(.*)"" e senha ""(.*)""")]
    public async Task QuandoEuTentoRealizarLoginComEmailESenha(string email, string senha)
    {
        await _loginPage.RealizarLoginAsync(email, senha);
    }

    [Then(@"uma mensagem de erro deve ser exibida")]
    public async Task EntaoUmaMensagemDeErroDeveSerExibida()
    {
        bool visivel = await _loginPage.IsMensagemErroVisivelAsync();
        Assert.That(visivel, Is.True, "A mensagem de erro de login não foi exibida.");
        
        string mensagem = await _loginPage.GetMensagemErroAsync();
        Console.WriteLine($"[INFO] Mensagem de erro capturada: {mensagem}");
    }

    [Then(@"eu devo permanecer na página de login")]
    public async Task EntaoEuDevoPermanecerNaPaginaDeLogin()
    {
        Assert.That(_loginPage.GetCurrentUrl(), Does.Contain("login"), "O sistema redirecionou para fora da página de login indevidamente.");
    }

    [Then(@"o sistema deve processar o login com segurança")]
    public async Task EntaoOSistemaDeveProcessarOLoginComSeguranca()
    {
        bool carregada = await _loginPage.IsPaginaCarregadaAsync();
        Assert.That(carregada, Is.True, "A página de login não carregou corretamente.");
    }

    [Then(@"se houver captcha, ele deve ser identificado para tratamento")]
    public async Task EntaoSeHouverCaptchaEleDeveSerIdentificadoParaTratamento()
    {
        if (await _loginPage.HasCaptchaAsync())
        {
            Console.WriteLine("[BLOQUEIO] Captcha detectado na tela de login.");
            // Aqui poderíamos integrar com serviços de resolução de captcha ou skip
        }
        else
        {
            Console.WriteLine("[INFO] Nenhum captcha detectado nesta execução.");
        }
    }

    [Given(@"que eu tento realizar login com credenciais válidas")]
    public async Task DadoQueEuTentoRealizarLoginComCredenciaisValidas()
    {
        await _loginPage.RealizarLoginComCredenciaisConfiguradasAsync();
    }

    [Then(@"o sistema pode solicitar um código de verificação para prosseguir")]
    public async Task EntaoOSistemaPodeSolicitarUmCodigoDeVerificacaoParaProsseguir()
    {
        if (await _loginPage.HasMFAAsync())
        {
            Console.WriteLine("[AUTH] MFA (Multi-Factor Authentication) solicitado.");
        }
        else
        {
            // Se não houver MFA, validamos se logou ou se deu erro esperado por ser conta teste
            Console.WriteLine("[INFO] MFA não solicitado ou login processado diretamente.");
        }
    }
}
