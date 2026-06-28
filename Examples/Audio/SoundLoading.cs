/*******************************************************************************************
*
*   raylib [audio] example - sound loading
*
*   Example complexity rating: [★☆☆☆] 1/4
*
*   Example originally created with raylib 1.1, last time updated with raylib 3.5
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2014-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using static Raylib_cs.Raylib;

namespace Examples.Audio;

public partial class SoundLoading
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [audio] example - sound loading");

        InitAudioDevice();      // Initialize audio device

        Sound fxWav = LoadSound("resources/audio/sound.wav");         // Load WAV audio file
        Sound fxOgg = LoadSound("resources/audio/target.ogg");        // Load OGG audio file

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            if (IsKeyPressed(KeyboardKey.Space))
            {
                PlaySound(fxWav);      // Play WAV sound
            }

            if (IsKeyPressed(KeyboardKey.Enter))
            {
                PlaySound(fxOgg);      // Play OGG sound
            }
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("Press SPACE to PLAY the WAV sound!", 200, 180, 20, Color.LightGray);
            DrawText("Press ENTER to PLAY the OGG sound!", 200, 220, 20, Color.LightGray);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadSound(fxWav);     // Unload sound data
        UnloadSound(fxOgg);     // Unload sound data

        CloseAudioDevice();     // Close audio device

        CloseWindow();          // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
