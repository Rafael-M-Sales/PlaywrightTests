using Microsoft.Playwright;

namespace PlaywrightTests.Utils;

/// <summary>
/// Utilitário para captura e gerenciamento de screenshots durante a execução dos testes.
/// Oferece métodos para captura de tela inteira, de elementos específicos e comparação visual.
/// </summary>
public static class ScreenshotHelper
{
    /// <summary>
    /// Captura screenshot de tela inteira e retorna os bytes da imagem.
    /// Útil para anexar em relatórios Allure.
    /// </summary>
    /// <param name="page">Instância da página do Playwright</param>
    /// <returns>Bytes do screenshot capturado</returns>
    public static async Task<byte[]> CaptureFullPageBytesAsync(IPage page)
    {
        return await page.ScreenshotAsync(new PageScreenshotOptions
        {
            FullPage = true
        });
    }

    /// <summary>
    /// Captura screenshot de um elemento específico.
    /// Útil para validações visuais de componentes isolados.
    /// </summary>
    /// <param name="locator">Localizador do elemento alvo</param>
    /// <param name="path">Caminho para salvar o screenshot</param>
    public static async Task CaptureElementScreenshotAsync(ILocator locator, string path)
    {
        await locator.ScreenshotAsync(new LocatorScreenshotOptions
        {
            Path = path
        });
    }

    /// <summary>
    /// Captura screenshot para uso em comparação visual (baseline).
    /// Salva em diretório específico de snapshots visuais.
    /// </summary>
    /// <param name="page">Instância da página</param>
    /// <param name="testName">Nome do teste para identificação do baseline</param>
    /// <param name="baselinePath">Diretório base para armazenamento de baselines</param>
    /// <returns>Caminho do arquivo salvo</returns>
    public static async Task<string> CaptureVisualBaselineAsync(
        IPage page, string testName, string baselinePath = "VisualBaselines")
    {
        var fullPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", baselinePath));
        Directory.CreateDirectory(fullPath);

        var filePath = Path.Combine(fullPath, $"{testName}.png");

        await page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = filePath,
            FullPage = true
        });

        return filePath;
    }
}
