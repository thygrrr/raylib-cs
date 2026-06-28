using Examples.Core;
using Examples.Models;
using Examples.Shapes;

namespace Examples;

/// <summary>
/// Browser registry guardrails. Desktop examples are registered in <see cref="ExampleList.AllExamples"/>.
/// </summary>
public static class ExampleRegistry
{
    /// <summary>
    /// Total desktop examples in <see cref="ExampleList.AllExamples"/> (Program.cs).
    /// Update when adding or removing a desktop-only example.
    /// </summary>
    public const int DesktopExampleCount = 114;

    /// <summary>Desktop examples omitted from the browser host (platform limitations).</summary>
    public static readonly Type[] DesktopExcludedFromBrowser =
    [
        typeof(DropFiles),
        typeof(SkyboxDemo),
    ];

    /// <summary>Browser-only shape examples not registered for desktop CLI runs.</summary>
    public static readonly Type[] BrowserOnly =
    [
        typeof(DrawCircleSector),
        typeof(DrawRectangleRounded),
        typeof(DrawRing),
    ];

    public static int ExpectedBrowserCount =>
        DesktopExampleCount - DesktopExcludedFromBrowser.Length + BrowserOnly.Length;

    public static void ValidateBrowserCount(int registeredCount)
    {
        var expected = ExpectedBrowserCount;
        if (registeredCount != expected)
        {
            throw new InvalidOperationException(
                $"Browser host registered {registeredCount} examples but expected {expected} " +
                $"({DesktopExampleCount} desktop - {DesktopExcludedFromBrowser.Length} excluded " +
                $"+ {BrowserOnly.Length} browser-only)."
            );
        }
    }
}
