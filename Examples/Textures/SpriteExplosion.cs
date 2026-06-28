/*******************************************************************************************
*
*   raylib [textures] example - sprite explosion
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 2.5, last time updated with raylib 3.5
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2019-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Textures;

public partial class SpriteExplosion
{
    const int NumFramesPerLine = 5;
    const int NumLines = 5;

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [textures] example - sprite explosion");
        InitAudioDevice();

        // Load explosion sound
        Sound fxBoom = LoadSound("resources/audio/boom.wav");

        // Load explosion texture
        Texture2D explosion = LoadTexture("resources/explosion.png");

        // Init variables for animation
        int frameWidth = explosion.Width / NumFramesPerLine;   // Sprite one frame rectangle width
        int frameHeight = explosion.Height / NumLines;         // Sprite one frame rectangle height
        int currentFrame = 0;
        int currentLine = 0;

        Rectangle frameRec = new(0, 0, frameWidth, frameHeight);
        Vector2 position = new(0.0f, 0.0f);

        bool active = false;
        int framesCounter = 0;

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //---------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------

            // Check for mouse button pressed and activate explosion (if not active)
            if (IsMouseButtonPressed(MouseButton.Left) && !active)
            {
                position = GetMousePosition();
                active = true;

                position.X -= frameWidth / 2;
                position.Y -= frameHeight / 2;

                PlaySound(fxBoom);
            }

            // Compute explosion animation frames
            if (active)
            {
                framesCounter++;

                if (framesCounter > 2)
                {
                    currentFrame++;

                    if (currentFrame >= NumFramesPerLine)
                    {
                        currentFrame = 0;
                        currentLine++;

                        if (currentLine >= NumLines)
                        {
                            currentLine = 0;
                            active = false;
                        }
                    }

                    framesCounter = 0;
                }
            }

            frameRec.X = frameWidth * currentFrame;
            frameRec.Y = frameHeight * currentLine;
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Draw explosion required frame rectangle
            if (active)
            {
                DrawTextureRec(explosion, frameRec, position, Color.White);
            }

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(explosion);   // Unload texture
        UnloadSound(fxBoom);        // Unload sound

        CloseAudioDevice();

        CloseWindow();              // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
