using Microsoft.Playwright;

var authPath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "..", "SpotifyE2E.Tests", "playwright", ".auth", "spotify_state.json"
);

Directory.CreateDirectory(Path.GetDirectoryName(authPath)!);

Console.WriteLine("====================================================");
Console.WriteLine("  LOGIN TOOL - Spotify");
Console.WriteLine("====================================================");
Console.WriteLine("");
Console.WriteLine("Se abrirá un navegador.");
Console.WriteLine("1. Loguéate manualmente en Spotify.");
Console.WriteLine("2. Asegúrate de llegar a 'open.spotify.com' (Web Player).");
Console.WriteLine("3. Vuelve a ESTA terminal y presiona ENTER para guardar.");
Console.WriteLine("");

using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new()
{
    Headless = false,
    Args = new[] { "--disable-blink-features=AutomationControlled" }
});

var context = await browser.NewContextAsync(new()
{
    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36"
});

var page = await context.NewPageAsync();
await page.GotoAsync("https://accounts.spotify.com/login");

Console.WriteLine(">>> Presiona ENTER cuando termines de loguearte en el navegador <<<");
Console.ReadLine();

Console.WriteLine("");
Console.WriteLine($"URL actual: {page.Url}");

await context.StorageStateAsync(new() { Path = authPath });

if (File.Exists(authPath))
{
    var size = new FileInfo(authPath).Length;
    Console.WriteLine($"✅ Sesión guardada en: {authPath}");
    Console.WriteLine($"   Tamaño: {size} bytes");
}
else
{
    Console.WriteLine("❌ ERROR: El archivo no se creó.");
}

await browser.CloseAsync();
Console.WriteLine("");
Console.WriteLine("Terminado. Cerrando...");