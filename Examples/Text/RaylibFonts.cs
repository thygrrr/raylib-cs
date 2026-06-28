/*******************************************************************************************
*
*   raylib [text] example - sprite fonts
*
*   Example complexity rating: [★☆☆☆] 1/4
*
*   NOTE: raylib is distributed with some free to use fonts (even for commercial pourposes!)
*         To view details and credits for those fonts, check raylib license file
*
*   Example originally created with raylib 1.7, last time updated with raylib 3.7
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2017-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Text;

public partial class RaylibFonts
{
    public const int MaxFonts = 8;

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [text] example - sprite fonts");

        // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
        Font[] fonts = new Font[MaxFonts];

        fonts[0] = LoadFont("resources/sprite_fonts/alagard.png");
        fonts[1] = LoadFont("resources/sprite_fonts/pixelplay.png");
        fonts[2] = LoadFont("resources/sprite_fonts/mecha.png");
        fonts[3] = LoadFont("resources/sprite_fonts/setback.png");
        fonts[4] = LoadFont("resources/sprite_fonts/romulus.png");
        fonts[5] = LoadFont("resources/sprite_fonts/pixantiqua.png");
        fonts[6] = LoadFont("resources/sprite_fonts/alpha_beta.png");
        fonts[7] = LoadFont("resources/sprite_fonts/jupiter_crash.png");

        string[] messages = new string[MaxFonts] {
                "ALAGARD FONT designed by Hewett Tsoi",
                "PIXELPLAY FONT designed by Aleksander Shevchuk",
                "MECHA FONT designed by Captain Falcon",
                "SETBACK FONT designed by Brian Kent (AEnigma)",
                "ROMULUS FONT designed by Hewett Tsoi",
                "PIXANTIQUA FONT designed by Gerhard Grossmann",
                "ALPHA_BETA FONT designed by Brian Kent (AEnigma)",
                "JUPITER_CRASH FONT designed by Brian Kent (AEnigma)"
            };

        int[] spacings = new int[MaxFonts] { 2, 4, 8, 4, 3, 4, 4, 1 };
        Vector2[] positions = new Vector2[MaxFonts];

        for (int i = 0; i < MaxFonts; i++)
        {
            float halfWidth = MeasureTextEx(fonts[i], messages[i], fonts[i].BaseSize * 2, spacings[i]).X / 2;
            positions[i].X = screenWidth / 2 - halfWidth;
            positions[i].Y = 60 + fonts[i].BaseSize + 45 * i;
        }

        // Small Y position corrections
        positions[3].Y += 8;
        positions[4].Y += 2;
        positions[7].Y -= 8;

        Color[] colors = new Color[MaxFonts] {
                Color.Maroon,
                Color.Orange,
                Color.DarkGreen,
                Color.DarkBlue,
                Color.DarkPurple,
                Color.Lime,
                Color.Gold,
                Color.Red
            };

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            // TODO: Update your variables here
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("free sprite fonts included with raylib", 220, 20, 20, Color.DarkGray);
            DrawLine(220, 50, 600, 50, Color.DarkGray);

            for (int i = 0; i < MaxFonts; i++)
            {
                DrawTextEx(fonts[i], messages[i], positions[i], fonts[i].BaseSize * 2, spacings[i], colors[i]);
            }

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------

        // Fonts unloading
        for (int i = 0; i < MaxFonts; i++)
        {
            UnloadFont(fonts[i]);
        }

        CloseWindow();                 // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}

