using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.RendererUtils;

public class RenderToTexture : CustomPass
{
    public LayerMask layerMask;
    public Material whiteMaterial;
    public RenderTexture outputTexture;

    RTHandle rtHandle;

    protected override void Setup(ScriptableRenderContext ctx, CommandBuffer cmd)
    {
        rtHandle = RTHandles.Alloc(outputTexture);
    }

    protected override void Execute(CustomPassContext ctx)
    {
        CoreUtils.SetRenderTarget(ctx.cmd, rtHandle, ClearFlag.Color, Color.black);
        
        // CoreUtils.SetRenderTarget(ctx.cmd, rtHandle, ClearFlag.Color, Color.magenta);
        HDUtils.DrawFullScreen(ctx.cmd, whiteMaterial, rtHandle);
        
        ShaderTagId[] shaderTagIds = new[] {
            // new ShaderTagId("SRPDefaultUnlit"), // Catch HDRP Unlit shaders
            new ShaderTagId("Forward"),          // Catch lit forward-rendered shaders
            new ShaderTagId("ShadowCaster"),  
        };

        var rendererListDesc = new RendererListDesc(
            shaderTagIds,
            ctx.cullingResults,
            ctx.hdCamera.camera)
        {
            rendererConfiguration = PerObjectData.None,
            layerMask = layerMask,
            renderQueueRange = RenderQueueRange.all,
            overrideMaterial = whiteMaterial,
        };
        
        var rendererList = ctx.renderContext.CreateRendererList(rendererListDesc);
        ctx.cmd.DrawRendererList(rendererList);
        
        // CoreUtils.DrawRendererList(ctx.renderContext, ctx.cmd, rendererList);
    }

    protected override void Cleanup()
    {
        RTHandles.Release(rtHandle);
    }
}
