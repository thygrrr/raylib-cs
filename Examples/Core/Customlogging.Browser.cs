#if BROWSER
using Examples;
using System.Runtime.InteropServices;

namespace Examples.Core;

public partial class CustomLogging : IExample
{
    private readonly BrowserAdapter _browserAdapter = new();

    public string Name => _browserAdapter.Name;

    public void Init()
    {
        _browserAdapter.Init();
    }

    public void Update()
    {
        _browserAdapter.Update();
    }

    public void Unload()
    {
        _browserAdapter.Unload();
    }

    private unsafe sealed class BrowserAdapter : IExample
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
}
#endif
