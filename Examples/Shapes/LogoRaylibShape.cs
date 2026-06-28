/*******************************************************************************************
*
*   raylib [shapes] example - logo raylib
*
*   Example complexity rating: [★☆☆☆] 1/4
*
*   Example originally created with raylib 1.0, last time updated with raylib 1.0
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2014-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using static Raylib_cs.Raylib;

namespace Examples.Shapes;

public partial class LogoRaylibShape
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [shapes] example - logo raylib");

        SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())        // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            // TODO: Update your variables here
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawRectangle(screenWidth / 2 - 128, screenHeight / 2 - 128, 256, 256, new Color(139, 71, 135, 255));
            DrawRectangle(screenWidth / 2 - 112, screenHeight / 2 - 112, 224, 224, Color.RayWhite);
            DrawText("raylib", screenWidth / 2 - 44, screenHeight / 2 + 28, 50, new Color(155, 79, 151, 255));
            DrawText("cs", screenWidth / 2 - 44, screenHeight / 2 + 58, 50, new Color(155, 79, 151, 255));

            DrawText("this is NOT a texture!", 350, 370, 10, Color.Gray);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        CloseWindow();        // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
