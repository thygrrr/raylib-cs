/*******************************************************************************************
*
*   raylib [textures] example - sprite button
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 2.5, last time updated with raylib 2.5
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

public partial class SpriteButton
{
    // Number of frames (rectangles) for the button sprite texture
    public const int NumFrames = 3;

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [textures] example - sprite button");

        InitAudioDevice();      // Initialize audio device

        Sound fxButton = LoadSound("resources/audio/buttonfx.wav");   // Load button sound
        Texture2D button = LoadTexture("resources/button.png"); // Load button texture

        // Define frame rectangle for drawing
        int frameHeight = button.Height / NumFrames;
        Rectangle sourceRec = new(0, 0, button.Width, frameHeight);

        // Define button bounds on screen
        Rectangle btnBounds = new(
            screenWidth / 2 - button.Width / 2,
            screenHeight / 2 - button.Height / NumFrames / 2,
            button.Width,
            frameHeight
        );

        // Button state: 0-NORMAL, 1-MOUSE_HOVER, 2-PRESSED
        int btnState = 0;

        // Button action should be activated
        bool btnAction = false;

        Vector2 mousePoint = new(0.0f, 0.0f);

        SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            mousePoint = GetMousePosition();
            btnAction = false;

            // Check button state
            if (CheckCollisionPointRec(mousePoint, btnBounds))
            {
                if (IsMouseButtonDown(MouseButton.Left))
                {
                    btnState = 2;
                }
                else
                {
                    btnState = 1;
                }

                if (IsMouseButtonReleased(MouseButton.Left))
                {
                    btnAction = true;
                }
            }
            else
            {
                btnState = 0;
            }

            if (btnAction)
            {
                PlaySound(fxButton);
                // TODO: Any desired action
            }

            // Calculate button frame rectangle to draw depending on button state
            sourceRec.Y = btnState * frameHeight;
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawTextureRec(button, sourceRec, new Vector2(btnBounds.X, btnBounds.Y), Color.White); // Draw button frame

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(button);  // Unload button texture
        UnloadSound(fxButton);  // Unload sound

        CloseAudioDevice();     // Close audio device

        CloseWindow();          // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
