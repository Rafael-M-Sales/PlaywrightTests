using Microsoft.Playwright;

namespace PlaywrightTests.Utils;

/// <summary>
/// Utilitário para aplicar destaque visual (highlight) em elementos durante a execução dos testes.
/// Aplica um contorno vermelho ao redor do elemento para facilitar a identificação visual
/// nas evidências (screenshots e vídeos).
/// </summary>
public static class HighlightHelper
{
    /// <summary>
    /// Aplica um contorno vermelho de 3px ao redor do elemento especificado.
    /// O destaque permanece visível por um breve período para captura de screenshot.
    /// </summary>
    /// <param name="page">Instância da página do Playwright</param>
    /// <param name="locator">Localizador do elemento a ser destacado</param>
    /// <param name="durationMs">Duração do destaque em milissegundos (padrão: 500ms)</param>
    public static async Task HighlightElementAsync(IPage page, ILocator locator, int durationMs = 500)
    {
        try
        {
            // Aplica o contorno vermelho via JavaScript
            await locator.EvaluateAsync(@"(element) => {
                element.style.outline = '3px solid red';
                element.style.outlineOffset = '2px';
            }");

            // Aguarda o tempo configurado para que o screenshot capture o highlight
            await Task.Delay(durationMs);
        }
        catch (Exception)
        {
            // Elemento pode não estar visível ou acessível — segue sem interromper o teste
        }
    }

    /// <summary>
    /// Remove o destaque visual de um elemento.
    /// </summary>
    /// <param name="locator">Localizador do elemento</param>
    public static async Task RemoveHighlightAsync(ILocator locator)
    {
        try
        {
            await locator.EvaluateAsync(@"(element) => {
                element.style.outline = '';
                element.style.outlineOffset = '';
            }");
        }
        catch (Exception)
        {
            // Ignora erros silenciosamente
        }
    }

    /// <summary>
    /// Destaca o elemento, realiza uma ação e depois remove o destaque.
    /// Padrão recomendado para uso nos Page Objects.
    /// </summary>
    /// <param name="page">Instância da página</param>
    /// <param name="locator">Localizador do elemento</param>
    /// <param name="action">Ação a ser executada no elemento</param>
    public static async Task HighlightAndActAsync(IPage page, ILocator locator, Func<Task> action)
    {
        await HighlightElementAsync(page, locator);
        await action();
        await RemoveHighlightAsync(locator);
    }
}
