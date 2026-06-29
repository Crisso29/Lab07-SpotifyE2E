using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Playlists;

[TestFixture]
public class TC002_EditarPlaylist : BaseTest
{
    // Valores iniciales
    private const string NombreInicial = "Playlist Original Crisso";
    private const string DescripcionInicial = "Descripción inicial antes de editar";

    // Valores nuevos después de editar
    private const string NombreEditado = "Playlist Editada - Vinchos QA";
    private const string DescripcionEditada = 
        "Descripción actualizada con éxito durante el test E2E del Lab 07.";

    [Test]
    [Description("TC-002: Edición exitosa de nombre y descripción | Técnica: PE-Válida | Prioridad: Alta")]
    public async Task EditarNombreYDescripcionExitosamente()
    {
        // ARRANGE — crear playlist con datos iniciales
        var home = new HomePage(Page);
        var playlist = new PlaylistPage(Page);
        await home.IrAlHomeAsync();
        await home.ClickCrearListaAsync();
        await playlist.EditarDetallesAsync(NombreInicial, DescripcionInicial);

        // ACT — editar la playlist con datos nuevos
        await playlist.EditarDetallesAsync(NombreEditado, DescripcionEditada);

        // ASSERT — verificar que el título refleja el nombre EDITADO
        var tituloActual = await playlist.TituloPlaylist.InnerTextAsync();
        Assert.That(tituloActual, Is.EqualTo(NombreEditado),
            $"El título no se actualizó. Esperado: '{NombreEditado}' | Actual: '{tituloActual}'");
    }
}