using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace SpotifyE2E.Tests.Base;

[Parallelizable(ParallelScope.Self)]
[Retry(2)]  // ← Si el test falla, lo reintenta hasta 2 veces más
public abstract class BaseTest : PageTest
{
    [OneTimeSetUp]
    public void SetHeaded()
    {
        Environment.SetEnvironmentVariable("HEADED", "1");
    }
}