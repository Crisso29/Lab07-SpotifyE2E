using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Busqueda;

[TestFixture]
public class TC009_BusquedaRegional : BaseTest
{
    private const string TerminoRegional = "Vinchos Ayacucho";

    [Test]
    [Description("TC-009: Búsqueda con término regional | Técnica: PE-Inválida | Prioridad: Media")]
    public async Task BusquedaTerminoRegionalNoGeneraError()
    {
        // ARRANGE
        var search = new SearchPage(Page);
        await search.IrABusquedaAsync();

        // ACT
        await search.BuscarAsync(TerminoRegional);

        // ASSERT 1 — la página NO crashea (sigue siendo accesible)
        var urlActual = Page.Url;
        TestContext.Out.WriteLine($"Término buscado: '{TerminoRegional}'");
        TestContext.Out.WriteLine($"URL actual: {urlActual}");

        Assert.That(urlActual, Does.Contain("search"),
            "La página de búsqueda no respondió correctamente");

        // ASSERT 2 — el input sigue funcional (sistema estable)
        var inputVisible = await search.InputBusqueda.IsVisibleAsync();
        Assert.That(inputVisible, Is.True,
            "El input de búsqueda dejó de ser visible (posible crash)");

        // INFO — log de resultados (no es aserción, solo info)
        var totalResultados = await search.ContarResultadosAsync();
        TestContext.Out.WriteLine($"Resultados encontrados: {totalResultados}");
        TestContext.Out.WriteLine("Sistema respondió sin errores ante término regional.");
    }
}