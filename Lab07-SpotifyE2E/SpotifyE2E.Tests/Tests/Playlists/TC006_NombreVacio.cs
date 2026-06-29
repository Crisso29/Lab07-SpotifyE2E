using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Playlists;

[TestFixture]
public class TC006_NombreVacio : BaseTest
{
    private const string NombreInicial = "Nombre temporal";
    private const string DescripcionInicial = "Test edge case - campo nombre vacio";

    [Test]
    [Description("TC-006: Nombre vacío → error obligatorio | Técnica: Edge Case | Prioridad: Alta")]
    public async Task NombreVacioMuestraMensajeDeError()
    {
        // ARRANGE — crear playlist con nombre inicial válido
        var home = new HomePage(Page);
        var playlist = new PlaylistPage(Page);
        await home.IrAlHomeAsync();
        await home.ClickCrearListaAsync();
        await playlist.EditarDetallesAsync(NombreInicial, DescripcionInicial);

        // ACT — abrir modal de edición y borrar el nombre
        await playlist.TituloPlaylist.ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        // Limpiar el campo nombre (FillAsync con string vacío)
        await playlist.InputNombre.FillAsync("");
        await Page.WaitForTimeoutAsync(500);

        // Intentar guardar con nombre vacío
        await playlist.BotonGuardar.ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        // ASSERT — debe aparecer el mensaje de error
        await Expect(playlist.MensajeErrorNombreVacio).ToBeVisibleAsync();

        TestContext.Out.WriteLine("✅ Mensaje de error mostrado correctamente");
    }
}