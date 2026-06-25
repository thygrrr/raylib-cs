namespace Examples;

/// <summary>
/// A browser-runnable raylib example. The host owns the window (InitWindow/CloseWindow) and the
/// frame loop; examples never block. Loop-spanning state from the original example's Main becomes
/// instance fields here.
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
