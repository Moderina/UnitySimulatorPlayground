using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Overlay : CustomPass  
{
    public Material overlayMaterial;

    protected override void Execute(CustomPassContext ctx)
    {
        if (overlayMaterial == null) return;

        // Draw the texture fullscreen
        HDUtils.DrawFullScreen(ctx.cmd, overlayMaterial, ctx.cameraColorBuffer, ctx.cameraDepthBuffer);
    }
}
