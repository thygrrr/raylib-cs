using System;
using static Raylib_cs.Raylib;

namespace Examples;

public abstract class Example : IExample
{
    public abstract string Name { get; }

    public virtual void Init()
    {
    }

    public abstract void Update();

    public virtual void Unload()
    {
    }

    public int RunDesktop(
        int width,
        int height,
        string title,
        int targetFps = 60,
        Action? beforeWindowInit = null
    )
    {
        beforeWindowInit?.Invoke();
        InitWindow(width, height, title);
        SetTargetFPS(targetFps);

        try
        {
            Init();

            while (!WindowShouldClose())
            {
                Update();
            }
        }
        finally
        {
            Unload();
            CloseWindow();
        }

        return 0;
    }
}
