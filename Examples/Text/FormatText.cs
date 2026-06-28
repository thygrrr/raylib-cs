/*******************************************************************************************
*
*   raylib [text] example - format text
*
*   Example complexity rating: [★☆☆☆] 1/4
*
*   Example originally created with raylib 1.1, last time updated with raylib 3.0
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2014-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using static Raylib_cs.Raylib;

namespace Examples.Text;

public partial class FormatText
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [text] example - format text");

        int score = 100020;
        int hiscore = 200450;
        int lives = 5;

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

            DrawText($"Score: {score:D8}", 200, 80, 20, Color.Red);

            DrawText($"HiScore: {hiscore:D8}", 200, 120, 20, Color.Green);

            DrawText($"Lives: {lives:D2}", 200, 160, 40, Color.Blue);

            DrawText($"Elapsed Time: {GetFrameTime() * 1000:F2} ms", 200, 220, 20, Color.Black);

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
