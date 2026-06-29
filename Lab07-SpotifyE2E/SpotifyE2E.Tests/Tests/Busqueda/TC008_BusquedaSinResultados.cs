using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Busqueda;

[TestFixture]
public class TC008_BusquedaSinResultados : BaseTest
{
    // Cadena sin sentido alguno
    private const string TerminoSinSentido = "xkqzlmpaowsi";

    [Test]
    [Category("KnownBug")]
    [Description("TC-008: Término sin sentido sin resultados | Técnica: PE-Inválida | ESTADO: FAIL (Bug Spotify)")]
    public async Task BusquedaSinSentidoDebeMostrarMensajeSinResultados()
    {
        // ARRANGE
        var search = new SearchPage(Page);
        await search.IrABusquedaAsync();

        // ACT
        await search.BuscarAsync(TerminoSinSentido);

        // ASSERT — esperamos 0 resultados (comportamiento esperado)
        var totalResultados = await search.ContarResultadosAsync();
        TestContext.Out.WriteLine($"Término buscado: '{TerminoSinSentido}'");
        TestContext.Out.WriteLine($"Total resultados: {totalResultados}");
        TestContext.Out.WriteLine($"Esperado: 0 (no debería haber resultados)");
        TestContext.Out.WriteLine($"NOTA: Si este test falla, confirma el BUG de Spotify (búsqueda fuzzy demasiado permisiva)");

        Assert.That(totalResultados, Is.EqualTo(0),
            $"BUG CONFIRMADO: Spotify retorna {totalResultados} resultados para '{TerminoSinSentido}' " +
            "cuando debería mostrar mensaje 'sin resultados'. Búsqueda fuzzy demasiado permisiva.");
    }
}