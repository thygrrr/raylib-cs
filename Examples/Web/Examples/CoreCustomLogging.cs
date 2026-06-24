// Adapted for the browser from Examples/Core/Customlogging.cs
// NOTE: The desktop example formats the message via Logging.GetLogMessage, which P/Invokes the
// native C runtime vsnprintf (msvcrt/libc/libSystem) to expand the printf varargs. None of those
// exist in browser-wasm, so we inline a minimal callback that prints the raw (unformatted) format
// string instead. Messages without varargs render verbatim; ones with format specifiers show their
// literal "%..." markers, which is an acceptable trade-off for a browser demo of the callback hook.
// Output goes to the browser developer console (Console.WriteLine maps to console.log under wasm).
using System.Runtime.InteropServices;

namespace Examples.Web;

public unsafe class CoreCustomLogging : IWebExample
{
    public string Name => "Core / Custom Logging";

    [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    private static void LogCustom(int logLevel, sbyte* text, sbyte* args)
    {
        string message = Marshal.PtrToStringUTF8(new IntPtr(text)) ?? string.Empty;
        Console.WriteLine("Custom " + message);
    }

    public void Init()
    {
        // Set our custom logger so everything raylib logs uses it instead of its internal one
        SetTraceLogCallback(&LogCustom);
        TraceLog(TraceLogLevel.Info, "Custom logging enabled for this example");
    }

    public void Update()
    {
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText("Check out the console output to see the custom logger in action!", 60, 200, 20, Color.LightGray);

        EndDrawing();
    }

    public void Unload()
    {
        // NOTE: The host's default raylib logger handles formatting; we leave the callback in place
        // since the browser has no native vsnprintf to restore a printf-style logger to.
    }
}
