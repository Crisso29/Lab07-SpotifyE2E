using Microsoft.Playwright;

namespace SpotifyE2E.Tests.Pages;

public class HomePage
{
    private readonly IPage _page;

    public HomePage(IPage page)
    {
        _page = page;
    }

    public async Task IrAlHomeAsync()
    {
        await _page.GotoAsync("https://open.spotify.com/", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = 60000
        });
        await _page.WaitForTimeoutAsync(3000);
    }

    public ILocator BotonCrear =>
        _page.GetByRole(AriaRole.Button, new() { Name = "Crear" });

    public ILocator OpcionLista =>
        _page.Locator("[aria-describedby='subtitle-global-create-playlist']");

    public async Task ClickCrearListaAsync()
    {
        await BotonCrear.ClickAsync();
        await _page.WaitForTimeoutAsync(800);
        await OpcionLista.ClickAsync();
        await _page.WaitForTimeoutAsync(3000);
    }
}