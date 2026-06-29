using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace SpotifyE2E.Tests.Base;

[Parallelizable(ParallelScope.Self)]
public abstract class BaseTest : PageTest
{
    public static readonly string AuthFile =
        Path.Combine(Directory.GetCurrentDirectory(), "playwright", ".auth", "spotify_state.json");

    [OneTimeSetUp]
    public void SetHeaded()
    {
        Environment.SetEnvironmentVariable("HEADED", "1");
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            StorageStatePath = AuthFile,
            ViewportSize = new ViewportSize { Width = 1280, Height = 720 },
            Locale = "es-ES"
        };
    }
}