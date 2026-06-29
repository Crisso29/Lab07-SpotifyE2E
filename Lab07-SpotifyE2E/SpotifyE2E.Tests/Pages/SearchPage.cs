using Microsoft.Playwright;

namespace SpotifyE2E.Tests.Pages;

public class SearchPage
{
    private readonly IPage _page;

    public SearchPage(IPage page)
    {
        _page = page;
    }

    public ILocator InputBusqueda =>
        _page.Locator("[data-testid='search-input']");

    public ILocator ContenedorResultados =>
        _page.GetByRole(AriaRole.Grid, new() { Name = "Search results" });

    public ILocator FilasResultado =>
        ContenedorResultados.GetByRole(AriaRole.Row);

    public async Task IrABusquedaAsync()
    {
        await _page.GotoAsync("https://open.spotify.com/search", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Commit,  // más permisivo: solo espera que el servidor responda
            Timeout = 90000                      // 90 segundos
        });
        // Esperar explícitamente a que el input esté listo
        await InputBusqueda.WaitForAsync(new() { Timeout = 60000 });
        await _page.WaitForTimeoutAsync(2000);
    }

    public async Task BuscarAsync(string termino)
    {
        await InputBusqueda.ClickAsync();
        await InputBusqueda.FillAsync(termino);
        await _page.Keyboard.PressAsync("Enter");
        await _page.WaitForTimeoutAsync(3000);
    }

    public async Task<int> ContarResultadosAsync()
    {
        try
        {
            return await FilasResultado.CountAsync();
        }
        catch
        {
            return 0;
        }
    }
}