using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Playlists;

[TestFixture]
public class TC003_DescripcionExcesiva : BaseTest
{
    private const string NombrePlaylist = "TC003 - Test Truncado";
    private const int LimiteEsperado = 300;

    [Test]
    [Description("TC-003: Descripción supera 300 chars (trunca) | Técnica: PE-Inválida | Prioridad: Alta")]
    public async Task DescripcionMayorA300CaracteresSeTrunca()
    {
        // ARRANGE — generamos una descripción de 500 caracteres
        var descripcionExcesiva = new string('A', 500);
        
        var home = new HomePage(Page);
        var playlist = new PlaylistPage(Page);
        await home.IrAlHomeAsync();
        await home.ClickCrearListaAsync();

        // ACT — intentar guardar con descripción de 500 chars
        await playlist.EditarDetallesAsync(NombrePlaylist, descripcionExcesiva);

        // ASSERT — el campo debe contener exactamente 300 chars (truncado)
        // Reabrimos el modal para leer el valor que quedó guardado
        await playlist.TituloPlaylist.ClickAsync();
        await Page.WaitForTimeoutAsync(1500);

        var valorGuardado = await playlist.InputDescripcion.InputValueAsync();
        var longitudReal = valorGuardado.Length;

        TestContext.Out.WriteLine($"Longitud ingresada: 500");
        TestContext.Out.WriteLine($"Longitud guardada: {longitudReal}");
        TestContext.Out.WriteLine($"Límite esperado: {LimiteEsperado}");

        Assert.That(longitudReal, Is.LessThanOrEqualTo(LimiteEsperado),
            $"La descripción NO se truncó. Se guardaron {longitudReal} chars (esperado ≤{LimiteEsperado})");
    }
}