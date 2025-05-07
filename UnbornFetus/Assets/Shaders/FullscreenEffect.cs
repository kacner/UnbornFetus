using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullscreenFeature : ScriptableRendererFeature
{
    class FullscreenPass : ScriptableRenderPass
    {
        public Material effectMaterial;
        public int _PixelSize = 5;
        public float DitheringIntensity = 1f;
        public float colorIntensity = 1f;
        public float ColorAmount = 1f;
        private RenderTargetHandle tempTexture;

        public FullscreenPass()
        {
            tempTexture.Init("_TempFullscreenTex");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (effectMaterial == null)
                return;

            // **Now** we grab the camera's color target inside Execute:
            RenderTargetIdentifier src = renderingData.cameraData.renderer.cameraColorTarget;
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;

            var cmd = CommandBufferPool.Get("FullscreenEffect");
            cmd.GetTemporaryRT(tempTexture.id, desc);

            effectMaterial.SetFloat("_Intensity", colorIntensity);
            effectMaterial.SetFloat("_DitheringIntensity", DitheringIntensity);
            effectMaterial.SetFloat("_PixelSize", _PixelSize);
            effectMaterial.SetFloat("_ColorAmount", ColorAmount);

            // Blit: src → temp with effect, then temp → src
            cmd.Blit(src, tempTexture.Identifier(), effectMaterial);
            cmd.Blit(tempTexture.Identifier(), src);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [System.Serializable]
    public class FullscreenSettings
    {
        public Material effectMaterial = null;
        [Range(0, 1)] public float intensity = 1f;
        [Range(0, 0.2f)] public float DitheringIntensity = 1f;
        [Range(1, 500)] public int PixelSize = 5;
        [Range(0, 255)] public float ColorAmount = 1f;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public FullscreenSettings settings = new FullscreenSettings();
    FullscreenPass pass;

    public override void Create()
    {
        pass = new FullscreenPass
        {
            effectMaterial = settings.effectMaterial,
            colorIntensity = settings.intensity,
            DitheringIntensity = settings.DitheringIntensity,
            _PixelSize = settings.PixelSize,
            ColorAmount = settings.ColorAmount,
            renderPassEvent = settings.renderPassEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.effectMaterial == null)
            return;

        // We no longer grab cameraColorTarget here!
        // We only enqueue the pass; it will pull src during Execute.
        renderer.EnqueuePass(pass);
    }
}
