/*******************************************************************************************
*
*   raylib [textures] example - image text
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 1.8, last time updated with raylib 4.0
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2017-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Textures;

public partial class ImageText
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [textures] example - image text");

        Image parrots = LoadImage("resources/parrots.png"); // Load image in CPU memory (RAM)

        // TTF Font loading with custom generation parameters
        Font font = LoadFontEx("resources/fonts/KAISG.ttf", 64, null, 0);

        // Draw over image using custom font
        ImageDrawTextEx(
            ref parrots,
            font,
            "[Parrots font drawing]",
            new Vector2(20, 20),
            font.BaseSize,
            0,
            Color.Red
        );

        Texture2D texture = LoadTextureFromImage(parrots);  // Image converted to texture, uploaded to GPU memory (VRAM)
        UnloadImage(parrots);   // Once image has been converted to texture and uploaded to VRAM, it can be unloaded from RAM

        Vector2 position = new(
            screenWidth / 2 - texture.Width / 2,
            screenHeight / 2 - texture.Height / 2 - 20
        );

        bool showFont = false;

        SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            if (IsKeyDown(KeyboardKey.Space))
            {
                showFont = true;
            }
            else
            {
                showFont = false;
            }
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            if (!showFont)
            {
                // Draw texture with text already drawn inside
                DrawTextureV(texture, position, Color.White);

                // Draw text directly using sprite font
                Vector2 textPosition = new(position.X + 20, position.Y + 20 + 280);
                DrawTextEx(font, "[Parrots font drawing]", textPosition, font.BaseSize, 0, Color.White);
            }
            else
            {
                DrawTexture(font.Texture, screenWidth / 2 - font.Texture.Width / 2, 50, Color.Black);
            }

            DrawText("PRESS SPACE to SHOW FONT ATLAS USED", 290, 420, 10, Color.DarkGray);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(texture);     // Texture unloading

        UnloadFont(font);           // Unload custom font

        CloseWindow();              // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
