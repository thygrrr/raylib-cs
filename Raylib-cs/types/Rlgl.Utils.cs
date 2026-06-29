using System.Numerics;

namespace Raylib_cs;

public static unsafe partial class Rlgl
{
    /// <summary>Load depth texture/renderbuffer (to be attached to fbo)</summary>
    public static uint LoadTextureDepth(int width, int height, bool useRenderBuffer) =>
        LoadTextureDepth(width, height, useRenderBuffer ? 1 : 0);

    /// <summary>Attach texture/renderbuffer to a framebuffer</summary>
    public static void FramebufferAttach(
        uint fboId,
        uint texId,
        FramebufferAttachType attachType,
        FramebufferAttachTextureType texType,
        int mipLevel
    ) => FramebufferAttach(fboId, texId, (int)attachType, (int)texType, mipLevel);

    /// <summary>Set shader value matrices</summary>
    public static void SetUniformMatrices(int locIndex, Matrix4x4[] mat)
    {
        fixed (Matrix4x4* p = mat)
        {
            SetUniformMatrices(locIndex, p, mat.Length);
        }
    }
}
