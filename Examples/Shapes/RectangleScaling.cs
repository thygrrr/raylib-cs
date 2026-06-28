/*******************************************************************************************
*
*   raylib [shapes] example - rectangle scaling
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 2.5, last time updated with raylib 2.5
*
*   Example contributed by Vlad Adrian (@demizdor) and reviewed by Ramon Santamaria (@raysan5)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2018-2025 Vlad Adrian (@demizdor) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Shapes;

public partial class RectangleScaling
{
    public const int MOUSE_SCALE_MARK_SIZE = 12;

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [shapes] example - rectangle scaling");

        Rectangle rec = new(100, 100, 200, 80);

        Vector2 mousePosition = new(0, 0);

        bool mouseScaleReady = false;
        bool mouseScaleMode = false;

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            mousePosition = GetMousePosition();

            Rectangle area = new(
                rec.X + rec.Width - MOUSE_SCALE_MARK_SIZE,
                rec.Y + rec.Height - MOUSE_SCALE_MARK_SIZE,
                MOUSE_SCALE_MARK_SIZE,
                MOUSE_SCALE_MARK_SIZE
            );

            if (CheckCollisionPointRec(mousePosition, area))
            {
                mouseScaleReady = true;
                if (IsMouseButtonPressed(MouseButton.Left))
                {
                    mouseScaleMode = true;
                }
            }
            else
            {
                mouseScaleReady = false;
            }

            if (mouseScaleMode)
            {
                mouseScaleReady = true;

                rec.Width = (mousePosition.X - rec.X);
                rec.Height = (mousePosition.Y - rec.Y);

                // Check minimum rec size
                if (rec.Width < MOUSE_SCALE_MARK_SIZE)
                {
                    rec.Width = MOUSE_SCALE_MARK_SIZE;
                }
                if (rec.Height < MOUSE_SCALE_MARK_SIZE)
                {
                    rec.Height = MOUSE_SCALE_MARK_SIZE;
                }

                // Check maximum rec size
                if (rec.Width > (GetScreenWidth() - rec.X))
                {
                    rec.Width = GetScreenWidth() - rec.X;
                }
                if (rec.Height > (GetScreenHeight() - rec.Y))
                {
                    rec.Height = GetScreenHeight() - rec.Y;
                }

                if (IsMouseButtonReleased(MouseButton.Left))
                {
                    mouseScaleMode = false;
                }
            }
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("Scale rectangle dragging from bottom-right corner!", 10, 10, 20, Color.Gray);

            DrawRectangleRec(rec, Fade(Color.Green, 0.5f));

            if (mouseScaleReady)
            {
                DrawRectangleLinesEx(rec, 1, Color.Red);
                DrawTriangle(
                    new Vector2(rec.X + rec.Width - MOUSE_SCALE_MARK_SIZE, rec.Y + rec.Height),
                    new Vector2(rec.X + rec.Width, rec.Y + rec.Height),
                    new Vector2(rec.X + rec.Width, rec.Y + rec.Height - MOUSE_SCALE_MARK_SIZE),
                    Color.Red
                );
            }

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
