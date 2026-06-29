using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Playlists;

[TestFixture]
public class TC004_NombreLimite100 : BaseTest
{
    private const int LongitudExacta = 100;
    private const string DescripcionPrueba = "Test AVL — Valor límite N (100 chars exactos)";

    [Test]
    [Description("TC-004: Nombre exactamente 100 chars (AVL N) | Técnica: AVL | Prioridad: Alta")]
    public async Task NombreCon100CaracteresExactosSeAceptaCorrectamente()
    {
        // ARRANGE — nombre con EXACTAMENTE 100 chars
        var nombre100 = new string('A', LongitudExacta);

        var home = new HomePage(Page);
        var playlist = new PlaylistPage(Page);
        await home.IrAlHomeAsync();
        await home.ClickCrearListaAsync();

        // ACT — intentar guardar con 100 chars
        await playlist.EditarDetallesAsync(nombre100, DescripcionPrueba);

        // ASSERT 1 — el título debe mostrar el nombre completo
        var tituloVisible = await playlist.TituloPlaylist.InnerTextAsync();
        TestContext.Out.WriteLine($"Longitud ingresada: {LongitudExacta}");
        TestContext.Out.WriteLine($"Longitud guardada: {tituloVisible.Length}");

        Assert.That(tituloVisible.Length, Is.EqualTo(LongitudExacta),
            $"El nombre no se guardó completo. Esperado: {LongitudExacta} chars | Actual: {tituloVisible.Length}");

        // ASSERT 2 — el contenido del título debe ser exactamente "AAAA...A" (100 veces)
        Assert.That(tituloVisible, Is.EqualTo(nombre100),
            "El contenido del nombre no coincide con lo ingresado");
    }
}