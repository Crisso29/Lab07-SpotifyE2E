using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Playlists;

[TestFixture]
public class TC005_NombreLimite101 : BaseTest
{
    private const int LongitudIngresada = 101;
    private const int LongitudMaximaPermitida = 100;
    private const string DescripcionPrueba = "Test AVL — Valor límite N+1 (101 chars, debe bloquearse)";

    [Test]
    [Description("TC-005: Nombre 101 chars bloqueado (AVL N+1) | Técnica: AVL | Prioridad: Alta")]
    public async Task NombreCon101CaracteresSeBloqueaYTruncahasta100()
    {
        // ARRANGE — nombre con 101 chars (1 más del límite)
        var nombre101 = new string('B', LongitudIngresada);

        var home = new HomePage(Page);
        var playlist = new PlaylistPage(Page);
        await home.IrAlHomeAsync();
        await home.ClickCrearListaAsync();

        // ACT — intentar guardar con 101 chars
        await playlist.EditarDetallesAsync(nombre101, DescripcionPrueba);

        // ASSERT — el sistema debe haber truncado a máximo 100 chars
        var tituloVisible = await playlist.TituloPlaylist.InnerTextAsync();
        TestContext.Out.WriteLine($"Longitud ingresada: {LongitudIngresada}");
        TestContext.Out.WriteLine($"Longitud guardada: {tituloVisible.Length}");
        TestContext.Out.WriteLine($"Límite permitido: {LongitudMaximaPermitida}");

        Assert.That(tituloVisible.Length, Is.LessThanOrEqualTo(LongitudMaximaPermitida),
            $"El sistema NO bloqueó el carácter 101. Se guardaron {tituloVisible.Length} chars (esperado ≤{LongitudMaximaPermitida})");
    }
}