using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Busqueda;

[TestFixture]
public class TC007_BusquedaValida : BaseTest
{
    private const string TerminoBusqueda = "Dua Lipa";

    [Test]
    [Description("TC-007: Búsqueda válida 'Dua Lipa' | Técnica: PE-Válida | Prioridad: Alta")]
    public async Task BuscarArtistaValidoMuestraResultados()
    {
        // ARRANGE
        var search = new SearchPage(Page);
        await search.IrABusquedaAsync();

        // ACT
        await search.BuscarAsync(TerminoBusqueda);

        // ASSERT 1 — debe haber resultados
        var totalResultados = await search.ContarResultadosAsync();
        TestContext.Out.WriteLine($"Total de resultados encontrados: {totalResultados}");

        Assert.That(totalResultados, Is.GreaterThan(0),
            $"No se encontraron resultados para '{TerminoBusqueda}'");

        // ASSERT 2 — el nombre del artista debe aparecer al menos una vez
        var dualipaVisible = await Page.GetByText("Dua Lipa").First.IsVisibleAsync();
        Assert.That(dualipaVisible, Is.True,
            "El nombre 'Dua Lipa' no aparece en los resultados");
    }
}