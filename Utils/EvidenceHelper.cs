using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Utils;

/// <summary>
/// Utilitário centralizado para captura e organização de evidências.
/// Gerencia screenshots por step, vídeos e organização em pastas
/// seguindo o padrão: /Evidencias/YYYY-MM-DD/nome-do-cenario/
/// </summary>
public static class EvidenceHelper
{
    /// <summary>
    /// Gera o caminho completo para salvar evidências de um cenário específico.
    /// Cria automaticamente a estrutura de diretórios se não existir.
    /// Formato: Evidencias/YYYY-MM-DD/nome-do-cenario-gherkin/
    /// </summary>
    /// <param name="scenarioName">Nome do cenário Gherkin sendo executado</param>
    /// <returns>Caminho absoluto do diretório de evidências</returns>
    public static string GetEvidencePath(string scenarioName)
    {
        var dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
        var sanitizedName = SanitizeFolderName(scenarioName);
        var basePath = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..",
            TestSettings.EvidenciasPath,
            dateFolder,
            sanitizedName
        );

        var fullPath = Path.GetFullPath(basePath);
        Directory.CreateDirectory(fullPath);
        return fullPath;
    }

    /// <summary>
    /// Captura um screenshot da página atual e salva na pasta de evidências do cenário.
    /// O nome do arquivo segue o padrão: step_XX_descricao.png
    /// </summary>
    /// <param name="page">Instância da página do Playwright</param>
    /// <param name="scenarioName">Nome do cenário Gherkin</param>
    /// <param name="stepName">Nome/descrição do step atual</param>
    /// <param name="stepNumber">Número sequencial do step</param>
    /// <returns>Caminho completo do screenshot salvo</returns>
    public static async Task<string> CaptureScreenshotAsync(
        IPage page, string scenarioName, string stepName, int stepNumber)
    {
        var evidencePath = GetEvidencePath(scenarioName);
        var sanitizedStep = SanitizeFolderName(stepName);
        var fileName = $"step_{stepNumber:D2}_{sanitizedStep}.png";
        var filePath = Path.Combine(evidencePath, fileName);

        await page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = filePath,
            FullPage = true
        });

        Console.WriteLine($"[EVIDÊNCIA] Screenshot salvo: {filePath}");
        return filePath;
    }

    /// <summary>
    /// Captura screenshot em caso de falha do teste.
    /// O arquivo é nomeado com prefixo "FALHA_" para fácil identificação.
    /// </summary>
    /// <param name="page">Instância da página do Playwright</param>
    /// <param name="scenarioName">Nome do cenário que falhou</param>
    /// <param name="errorMessage">Mensagem de erro capturada</param>
    /// <returns>Caminho completo do screenshot de falha</returns>
    public static async Task<string> CaptureFailureScreenshotAsync(
        IPage page, string scenarioName, string errorMessage)
    {
        var evidencePath = GetEvidencePath(scenarioName);
        var timestamp = DateTime.Now.ToString("HHmmss");
        var fileName = $"FALHA_{timestamp}.png";
        var filePath = Path.Combine(evidencePath, fileName);

        await page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = filePath,
            FullPage = true
        });

        // Salva também o log de erro em arquivo texto
        var logPath = Path.Combine(evidencePath, $"FALHA_{timestamp}_log.txt");
        await File.WriteAllTextAsync(logPath, $"Cenário: {scenarioName}\nData/Hora: {DateTime.Now}\nErro: {errorMessage}");

        Console.WriteLine($"[EVIDÊNCIA] Screenshot de falha salvo: {filePath}");
        return filePath;
    }

    /// <summary>
    /// Sanitiza o nome do cenário/step para ser usado como nome de pasta/arquivo.
    /// Remove caracteres especiais e substitui espaços por hifens.
    /// </summary>
    private static string SanitizeFolderName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(name.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
        return sanitized.Replace(" ", "-").ToLower().Trim('-');
    }
}
