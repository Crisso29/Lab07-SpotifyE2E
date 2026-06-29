using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Busqueda;

[TestFixture]
public class TC011_BusquedaVacia : BaseTest
{
    [Test]
    [Description("TC-011: Campo vacío / espacios | Técnica: Edge Case | Prioridad: Media")]
    public async Task BusquedaVaciaNoEjecutaConsulta()
    {
        // ARRANGE
        var search = new SearchPage(Page);
        await search.IrABusquedaAsync();

        // ACT — escribir solo espacios y presionar Enter
        await search.InputBusqueda.ClickAsync();
        await search.InputBusqueda.FillAsync("     "); // 5 espacios
        await Page.Keyboard.PressAsync("Enter");
        await Page.WaitForTimeoutAsync(2000);

        // ASSERT 1 — la URL sigue en /search (no se ejecutó búsqueda real)
        var urlActual = Page.Url;
        TestContext.Out.WriteLine($"URL después de buscar espacios: {urlActual}");

        Assert.That(urlActual, Does.Contain("search"),
            "La URL cambió inesperadamente con búsqueda vacía");

        // ASSERT 2 — NO debe haber grid de resultados visible
        var hayGridResultados = await search.ContenedorResultados.IsVisibleAsync();
        TestContext.Out.WriteLine($"¿Hay resultados visibles?: {hayGridResultados}");

        Assert.That(hayGridResultados, Is.False,
            "Spotify ejecutó búsqueda con campo vacío (no debería)");

        TestContext.Out.WriteLine("Sistema correctamente ignoró búsqueda vacía.");
    }
}