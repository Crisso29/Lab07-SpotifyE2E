using Microsoft.Playwright;

namespace SpotifyE2E.Tests.Pages;

public class PlaylistPage
{
    private readonly IPage _page;

    public PlaylistPage(IPage page)
    {
        _page = page;
    }

    // Título grande de la playlist - usa la clase headline-large que es única
    public ILocator TituloPlaylist =>
        _page.Locator("h1.encore-text-headline-large");

    // Campo nombre en el modal
    public ILocator InputNombre =>
        _page.Locator("[data-testid='playlist-edit-details-name-input']");

    // Campo descripción en el modal
    public ILocator InputDescripcion =>
        _page.Locator("[data-testid='playlist-edit-details-description-input']");

    // Botón Guardar del modal
    public ILocator BotonGuardar =>
        _page.GetByRole(AriaRole.Button, new() { Name = "Guardar" });
    
    // Mensaje de error cuando el nombre está vacío
    public ILocator MensajeErrorNombreVacio =>
        _page.GetByText("El nombre de la lista de reproducción es obligatorio");

    public async Task EditarDetallesAsync(string nombre, string descripcion)
    {
        // Click en el título → abre modal de edición
        await TituloPlaylist.ClickAsync();
        await _page.WaitForTimeoutAsync(1500);

        // Llenar nombre (FillAsync limpia primero automáticamente)
        await InputNombre.FillAsync(nombre);
        await _page.WaitForTimeoutAsync(500);

        // Llenar descripción
        await InputDescripcion.FillAsync(descripcion);
        await _page.WaitForTimeoutAsync(500);

        // Guardar
        await BotonGuardar.ClickAsync();
        await _page.WaitForTimeoutAsync(2000);
    }
}