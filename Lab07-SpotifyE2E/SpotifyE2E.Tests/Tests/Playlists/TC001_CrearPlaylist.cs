using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Playlists;

[TestFixture]
public class TC001_CrearPlaylist : BaseTest
{
    private const string NombrePlaylist = "Para Llullu sara";
    private const string DescripcionPlaylist = 
        "Huaynos y música andina tradicional para los momentos en el campo. " +
        "Dedicado a la comunidad de Vinchos, Ayacucho.";

    [Test]
    [Description("TC-001: Creación exitosa de playlist | Técnica: PE-Válida | Prioridad: Alta")]
    public async Task CrearPlaylistExitosamente()
    {
        // ARRANGE
        var home = new HomePage(Page);
        var playlist = new PlaylistPage(Page);
        await home.IrAlHomeAsync();

        // ACT
        await home.ClickCrearListaAsync();
        await playlist.EditarDetallesAsync(NombrePlaylist, DescripcionPlaylist);

        // ASSERT
        var url = Page.Url;
        Assert.That(url, Does.Contain("playlist"),
            $"Esperaba URL de playlist. URL actual: {url}");

        // Verifica que el título quedó con el nombre que pusimos
        var tituloVisible = await playlist.TituloPlaylist.InnerTextAsync();
        Assert.That(tituloVisible, Is.EqualTo(NombrePlaylist),
            $"El título no coincide. Esperado: '{NombrePlaylist}' | Actual: '{tituloVisible}'");
    }
}