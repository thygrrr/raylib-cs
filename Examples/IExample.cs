namespace Examples;

/// <summary>
/// A browser-runnable raylib example. The host owns the window (InitWindow/CloseWindow) and the
/// frame loop; examples never block. Loop-spanning state from the original example's Main becomes
/// instance fields here.
///
/// <para>
/// Desktop vs browser duplication: desktop examples run via <c>static Main()</c> with a blocking
/// <c>while (!WindowShouldClose())</c> loop. Browser examples live in <c>*.Browser.cs</c> partials
/// (compiled only under <c>BROWSER</c>) whose nested <c>BrowserAdapter</c> copies the same logic
/// into <see cref="Init"/>, <see cref="Update"/>, and <see cref="Unload"/>. This duplication is
/// intentional — the browser host drives one frame at a time from JavaScript. When changing an
/// example, update both the desktop <c>Main()</c> body and its <c>BrowserAdapter</c> copy.
/// </para>
/// </summary>
public interface IExample
{
    /// <summary>Display name shown in the navigation dropdown.</summary>
    string Name { get; }

    /// <summary>One-time setup (was the code before the original `while` loop), minus InitWindow.</summary>
    void Init();

    /// <summary>A single frame (was the body of the original `while` loop), including BeginDrawing/EndDrawing.</summary>
    void Update();

    /// <summary>Free resources (was the code after the loop), minus CloseWindow.</summary>
    void Unload();
}
