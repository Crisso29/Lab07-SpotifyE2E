using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Busqueda;

[TestFixture]
public class TC010_CadenaExcesiva : BaseTest
{
    private const int LongitudCadena = 800;

    [Test]
    [Description("TC-010: Cadena 800+ chars excesiva | Técnica: PE-Inválida | Prioridad: Media")]
    public async Task BusquedaConCadenaExcesivaNoGeneraCrash()
    {
        // ARRANGE — string de 800 caracteres
        var cadenaExcesiva = new string('Z', LongitudCadena);

        var search = new SearchPage(Page);
        await search.IrABusquedaAsync();

        // ACT
        await search.BuscarAsync(cadenaExcesiva);

        // ASSERT 1 — la página sigue accesible (no crash)
        var urlActual = Page.Url;
        TestContext.Out.WriteLine($"Longitud cadena ingresada: {LongitudCadena}");
        TestContext.Out.WriteLine($"URL actual: {urlActual}");

        Assert.That(urlActual, Does.Contain("spotify.com"),
            "La página crasheó o redirigió incorrectamente");

        // ASSERT 2 — el input sigue funcional
        var inputVisible = await search.InputBusqueda.IsVisibleAsync();
        Assert.That(inputVisible, Is.True,
            "El input de búsqueda dejó de ser visible (posible crash)");

        // INFO — qué quedó en el input (Spotify probablemente lo truncó)
        var valorInput = await search.InputBusqueda.InputValueAsync();
        TestContext.Out.WriteLine($"Longitud valor guardado en input: {valorInput.Length}");
        TestContext.Out.WriteLine("Sistema manejó la cadena excesiva sin crashear.");
    }
}
