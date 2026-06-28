/*******************************************************************************************
*
*   raylib [text] example - writing anim
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 1.4, last time updated with raylib 1.4
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2016-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using static Raylib_cs.Raylib;

namespace Examples.Text;

public partial class WritingAnim
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [text] example - writing anim");

        string message = "This sample illustrates a text writing\nanimation effect! Check it out! ;)";

        int framesCounter = 0;

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            if (IsKeyDown(KeyboardKey.Space))
            {
                framesCounter += 8;
            }
            else
            {
                framesCounter += 1;
            }

            if (IsKeyPressed(KeyboardKey.Enter))
            {
                framesCounter = 0;
            }
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText(message.SubText(0, framesCounter / 10), 210, 160, 20, Color.Maroon);

            DrawText("PRESS [ENTER] to RESTART!", 240, 260, 20, Color.LightGray);
            DrawText("HOLD [SPACE] to SPEED UP!", 239, 300, 20, Color.LightGray);

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
