using MyApp.AppServices.ErrorLogging;
using System.Diagnostics.CodeAnalysis;

namespace MyApp.WebApp.Pages.Dev;

// FUTURE: Remove this page once testing of error handling is complete.
[AllowAnonymous]
public class ThrowErrorModel(IErrorLogger errorLogger) : PageModel
{
    public string ShortCode { get; private set; } = string.Empty;

    public async Task OnGetAsync()
    {
        try
        {
            throw new TestException("Test handled exception");
        }
        catch (Exception e)
        {
            ShortCode = await errorLogger.LogErrorAsync(e);
        }
    }

    [SuppressMessage("Minor Code Smell",
        "S2325:Methods and properties that don\'t access instance data should be static")]
    public void OnGetUnhandled()
    {
        throw new TestException("Test unhandled exception");
    }

    public class TestException(string message) : Exception(message);
}
