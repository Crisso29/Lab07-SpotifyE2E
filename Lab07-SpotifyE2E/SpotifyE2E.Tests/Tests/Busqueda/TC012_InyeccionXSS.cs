using Microsoft.Playwright;
using SpotifyE2E.Tests.Base;
using SpotifyE2E.Tests.Pages;

namespace SpotifyE2E.Tests.Tests.Busqueda;

[TestFixture]
public class TC012_InyeccionXSS : BaseTest
{
    // Payload combinado: XSS + SQL injection
    private const string PayloadMalicioso = "<script>alert('XSS')</script>' OR 1=1--";

    [Test]
    [Description("TC-012: XSS + SQL injection sanitizado | Técnica: Edge Case (Security) | Prioridad: Crítica")]
    public async Task InyeccionXSSYSQLDebenSerSanitizadas()
    {
        // ARRANGE — escuchar diálogos (alerts) - si XSS funciona, este se dispara
        bool alertEjecutado = false;
        Page.Dialog += async (_, dialog) =>
        {
            alertEjecutado = true;
            TestContext.Out.WriteLine($"⚠️ ALERT DETECTADO: {dialog.Message}");
            await dialog.DismissAsync();
        };

        var search = new SearchPage(Page);
        await search.IrABusquedaAsync();

        // ACT — inyectar payload malicioso
        await search.BuscarAsync(PayloadMalicioso);

        // ASSERT 1 — el alert() NO se ejecutó (XSS bloqueado)
        TestContext.Out.WriteLine($"Payload inyectado: {PayloadMalicioso}");
        TestContext.Out.WriteLine($"¿Se ejecutó alert()?: {alertEjecutado}");

        Assert.That(alertEjecutado, Is.False,
            "VULNERABILIDAD XSS: Spotify ejecutó el script inyectado");

        // ASSERT 2 — la página sigue funcional (no crash por SQL injection)
        var urlActual = Page.Url;
        Assert.That(urlActual, Does.Contain("spotify.com"),
            "La página crasheó tras inyección SQL");

        // ASSERT 3 — el input sigue accesible
        var inputVisible = await search.InputBusqueda.IsVisibleAsync();
        Assert.That(inputVisible, Is.True,
            "El input dejó de funcionar tras la inyección");

        TestContext.Out.WriteLine("✅ Spotify sanitizó correctamente XSS + SQL injection");
    }
}