using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#if UNITY_2023_3_OR_NEWER
using UnityEngine.Rendering.RenderGraphModule;
#endif

namespace HighlightPlus
{
    public class HighlightPlusRenderPassFeature : ScriptableRendererFeature
    {
        public static int sortFrameCount;

        private class HighlightPass : ScriptableRenderPass
        {
            private class PassData
            {
                public Camera camera;
#if UNITY_2022_1_OR_NEWER
                public RTHandle colorTarget, depthTarget;
#else
                public RenderTargetIdentifier colorTarget, depthTarget;
#endif
#if UNITY_2023_3_OR_NEWER
                  public TextureHandle colorTexture, depthTexture;
#endif
                public bool clearStencil;
                public CommandBuffer cmd;
            }

            private readonly PassData passData = new();

            // far objects render first
            private class DistanceComparer : IComparer<HighlightEffect>
            {
                public Vector3 camPos;

                public int Compare(HighlightEffect e1, HighlightEffect e2)
                {
                    if (e1.sortingPriority < e2.sortingPriority) return -1;
                    if (e1.sortingPriority > e2.sortingPriority) return 1;
                    var e1Pos = e1.transform.position;
                    var dx1 = e1Pos.x - camPos.x;
                    var dy1 = e1Pos.y - camPos.y;
                    var dz1 = e1Pos.z - camPos.z;
                    var distE1 = dx1 * dx1 + dy1 * dy1 + dz1 * dz1 + e1.sortingOffset;
                    var e2Pos = e2.transform.position;
                    var dx2 = e2Pos.x - camPos.x;
                    var dy2 = e2Pos.y - camPos.y;
                    var dz2 = e2Pos.z - camPos.z;
                    var distE2 = dx2 * dx2 + dy2 * dy2 + dz2 * dz2 + e2.sortingOffset;
                    if (distE1 > distE2) return -1;
                    if (distE1 < distE2) return 1;
                    return 0;
                }
            }

            public bool usesCameraOverlay;

            private ScriptableRenderer renderer;
            private RenderTextureDescriptor cameraTextureDescriptor;
            private static DistanceComparer effectDistanceComparer = new();
            private bool clearStencil;

            public void Setup(HighlightPlusRenderPassFeature passFeature, ScriptableRenderer renderer)
            {
                renderPassEvent = passFeature.renderPassEvent;
                clearStencil = passFeature.clearStencil;
                this.renderer = renderer;
            }

#if UNITY_2023_3_OR_NEWER
            [Obsolete]
#endif
            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                this.cameraTextureDescriptor = cameraTextureDescriptor;
#if UNITY_2021_2_OR_NEWER
                ConfigureInput(ScriptableRenderPassInput.Depth);
#endif
            }

#if UNITY_2023_3_OR_NEWER
            [Obsolete]
#endif
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
#if UNITY_2022_1_OR_NEWER
                var cameraColorTarget = renderer.cameraColorTargetHandle;
                var cameraDepthTarget = renderer.cameraDepthTargetHandle;
#else
                RenderTargetIdentifier cameraColorTarget = renderer.cameraColorTarget;
                RenderTargetIdentifier cameraDepthTarget = renderer.cameraDepthTarget;
#endif
#if !UNITY_2021_2_OR_NEWER
                // In Unity 2021.2, when MSAA > 1, cameraDepthTarget is no longer cameraColorTarget
                if (!usesCameraOverlay && (cameraTextureDescriptor.msaaSamples > 1 || cam.cameraType == CameraType.SceneView)) {
                    cameraDepthTarget = cameraColorTarget;
                }
#endif

                passData.clearStencil = clearStencil;
                passData.camera = renderingData.cameraData.camera;
                passData.colorTarget = cameraColorTarget;
                passData.depthTarget = cameraDepthTarget;

                var cmd = CommandBufferPool.Get("Highlight Plus");
                cmd.Clear();

                passData.cmd = cmd;
                ExecutePass(passData);
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            private static void ExecutePass(PassData passData)
            {
                var count = HighlightEffect.effects.Count;

#if UNITY_EDITOR
// Safety check: in some circumstances, the render graph is executed when some components are null before being removed from the effects list
                if (!Application.isPlaying)
                {
                    HighlightEffect.effects.RemoveAll(t => t == null);
                    count = HighlightEffect.effects.Count;
                }
#endif
                if (count == 0) return;

                var cam = passData.camera;
                var camLayer = 1 << cam.gameObject.layer;
                var camType = cam.cameraType;

                if (!HighlightEffect.customSorting && ((camType == CameraType.Game && sortFrameCount++ % 10 == 0) ||
                                                       !Application.isPlaying))
                {
                    effectDistanceComparer.camPos = cam.transform.position;
                    HighlightEffect.effects.Sort(effectDistanceComparer);
                }

                for (var k = 0; k < count; k++)
                {
                    var effect = HighlightEffect.effects[k];

                    if (!(effect.ignoreObjectVisibility || effect.isVisible)) continue;

                    if (!effect.isActiveAndEnabled) continue;

                    if (camType == CameraType.Reflection && !effect.reflectionProbes) continue;

                    if ((effect.camerasLayerMask & camLayer) == 0) continue;

                    effect.SetCommandBuffer(passData.cmd);
                    effect.BuildCommandBuffer(passData.camera, passData.colorTarget, passData.depthTarget,
                        passData.clearStencil);
                    passData.clearStencil = false;
                }
            }

#if UNITY_2023_3_OR_NEWER
            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {

                using (var builder = renderGraph.AddUnsafePass<PassData>("Highlight Plus Pass RG", out var passData)) {

                    builder.AllowPassCulling(false);

                    passData.clearStencil = clearStencil;
                    passData.camera = frameData.Get<UniversalCameraData>().camera;

                    UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                    passData.colorTexture = resourceData.activeColorTexture;
                    passData.depthTexture = resourceData.activeDepthTexture;
                    
                    builder.UseTexture(resourceData.activeColorTexture, AccessFlags.ReadWrite);
                    builder.UseTexture(resourceData.activeDepthTexture, AccessFlags.Read);
                    builder.UseTexture(resourceData.cameraDepthTexture, AccessFlags.Read);

                    ConfigureInput(ScriptableRenderPassInput.Depth);

                    builder.SetRenderFunc((PassData passData, UnsafeGraphContext context) => {
                        CommandBuffer cmd = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);
                        passData.cmd = cmd;
                        passData.colorTarget = passData.colorTexture;
                        passData.depthTarget = passData.depthTexture;
                        ExecutePass(passData);
                    });
                }
            }
#endif
        }

        private HighlightPass renderPass;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

        [Tooltip(
            "Clears stencil buffer before rendering highlight effects. This option can solve compatibility issues with shaders that also use stencil buffers.")]
        public bool clearStencil;

        /// <summary>
        /// Makes the effects visible in Edit mode.
        /// </summary>
        [Tooltip("If enabled, effects will be visible also in Edit mode (when not in Play mode).")]
        public bool previewInEditMode = true;

        /// <summary>
        /// Makes the effects visible in Edit mode.
        /// </summary>
        [Tooltip(
            "If enabled, effects will be visible also in Preview camera (preview camera shown when a camera is selected in Editor).")]
        public bool showInPreviewCamera = true;

        public static bool installed;
        public static bool showingInEditMode;

        private const string PREVIEW_CAMERA_NAME = "Preview Camera";

        private void OnDisable()
        {
            installed = false;
        }

        public override void Create()
        {
            renderPass = new HighlightPass();
            VRCheck.Init();
        }

        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            showingInEditMode = previewInEditMode;
            var cam = renderingData.cameraData.camera;

#if UNITY_EDITOR
            if (!previewInEditMode && !Application.isPlaying) return;

            if (cam.cameraType == CameraType.Preview) return;

            if (!showInPreviewCamera && PREVIEW_CAMERA_NAME.Equals(cam.name)) return;
#endif

#if UNITY_2019_4_OR_NEWER
            if (renderingData.cameraData.renderType == CameraRenderType.Base)
                renderPass.usesCameraOverlay = cam.GetUniversalAdditionalCameraData().cameraStack.Count > 0;
#endif
            renderPass.Setup(this, renderer);
            renderer.EnqueuePass(renderPass);
            installed = true;
        }
    }
}