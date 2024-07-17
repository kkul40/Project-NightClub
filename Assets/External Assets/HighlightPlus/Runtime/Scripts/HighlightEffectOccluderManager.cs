using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace HighlightPlus
{
    public partial class HighlightEffect : MonoBehaviour
    {
        private static readonly List<HighlightSeeThroughOccluder> occluders = new();
        private static readonly Dictionary<Camera, int> occludersFrameCount = new();
        private static Material fxMatSeeThroughOccluder, fxMatDepthWrite;
        private static RaycastHit[] hits;

        /// <summary>
        /// True if the see-through is cancelled by an occluder using raycast method
        /// </summary>
        public bool IsSeeThroughOccluded(Camera cam)
        {
            if (rms == null) return false;

            // Compute bounds
            var bounds = new Bounds();
            for (var r = 0; r < rms.Length; r++)
                if (rms[r].renderer != null)
                {
                    if (bounds.size.x == 0)
                        bounds = rms[r].renderer.bounds;
                    else
                        bounds.Encapsulate(rms[r].renderer.bounds);
                }

            var pos = bounds.center;
            var camPos = cam.transform.position;
            var offset = pos - camPos;
            var maxDistance = Vector3.Distance(pos, camPos);
            if (hits == null || hits.Length == 0) hits = new RaycastHit[64];

            var occludersCount = occluders.Count;
            var hitCount = Physics.BoxCastNonAlloc(pos - offset, bounds.extents * 0.9f, offset.normalized, hits,
                Quaternion.identity, maxDistance);
            for (var k = 0; k < hitCount; k++)
            for (var j = 0; j < occludersCount; j++)
                if (hits[k].collider.transform == occluders[j].transform)
                    return true;

            return false;
        }

        public static void RegisterOccluder(HighlightSeeThroughOccluder occluder)
        {
            if (!occluders.Contains(occluder)) occluders.Add(occluder);
        }

        public static void UnregisterOccluder(HighlightSeeThroughOccluder occluder)
        {
            if (occluders.Contains(occluder)) occluders.Remove(occluder);
        }

        /// <summary>
        /// Test see-through occluders.
        /// </summary>
        /// <param name="cam">The camera to be tested</param>
        /// <returns>Returns true if there's no raycast-based occluder cancelling the see-through effect</returns>
        public bool RenderSeeThroughOccluders(CommandBuffer cb, Camera cam)
        {
            var occludersCount = occluders.Count;
            if (occludersCount == 0 || rmsCount == 0) return true;

            var useRayCastCheck = false;
            // Check if raycast method is needed
            for (var k = 0; k < occludersCount; k++)
            {
                var occluder = occluders[k];
                if (occluder == null || !occluder.isActiveAndEnabled) continue;
                if (occluder.mode == OccluderMode.BlocksSeeThrough &&
                    occluder.detectionMethod == DetectionMethod.RayCast)
                {
                    useRayCastCheck = true;
                    break;
                }
            }

            if (useRayCastCheck)
                if (IsSeeThroughOccluded(cam))
                    return false;

            // do not render see-through occluders more than once this frame per camera (there can be many highlight effect scripts in the scene, we only need writing to stencil once)
            int lastFrameCount;
            occludersFrameCount.TryGetValue(cam, out lastFrameCount);
            var currentFrameCount = Time.frameCount;
            if (currentFrameCount == lastFrameCount) return true;
            occludersFrameCount[cam] = currentFrameCount;

            if (fxMatSeeThroughOccluder == null)
            {
                InitMaterial(ref fxMatSeeThroughOccluder, "HighlightPlus/Geometry/SeeThroughOccluder");
                if (fxMatSeeThroughOccluder == null) return true;
            }

            if (fxMatDepthWrite == null)
            {
                InitMaterial(ref fxMatDepthWrite, "HighlightPlus/Geometry/JustDepth");
                if (fxMatDepthWrite == null) return true;
            }

            for (var k = 0; k < occludersCount; k++)
            {
                var occluder = occluders[k];
                if (occluder == null || !occluder.isActiveAndEnabled) continue;
                if (occluder.detectionMethod == DetectionMethod.Stencil)
                {
                    if (occluder.meshData == null) continue;
                    var meshDataLength = occluder.meshData.Length;
                    // Per renderer
                    for (var m = 0; m < meshDataLength; m++)
                    {
                        // Per submesh
                        var renderer = occluder.meshData[m].renderer;
                        if (renderer.isVisible)
                            for (var s = 0; s < occluder.meshData[m].subMeshCount; s++)
                                cb.DrawRenderer(renderer,
                                    occluder.mode == OccluderMode.BlocksSeeThrough
                                        ? fxMatSeeThroughOccluder
                                        : fxMatDepthWrite, s);
                    }
                }
            }

            return true;
        }

        private bool CheckOcclusion(Camera cam)
        {
            if (!perCameraOcclusionData.TryGetValue(cam, out var occlusionData))
            {
                occlusionData = new PerCameraOcclusionData();
                perCameraOcclusionData[cam] = occlusionData;
            }

            var now = GetTime();
            var frameCount = Time.frameCount; // ensure all cameras are checked this frame

            if (now - occlusionData.checkLastTime < seeThroughOccluderCheckInterval && Application.isPlaying &&
                occlusionData.occlusionRenderFrame != frameCount) return occlusionData.lastOcclusionTestResult;

            occlusionData.cachedOccluders.Clear();
            occlusionData.cachedOccluderCollider = null;

            if (rms == null || rms.Length == 0 || rms[0].renderer == null) return false;

            occlusionData.checkLastTime = now;
            occlusionData.occlusionRenderFrame = frameCount;

            var camPos = cam.transform.position;

            if (seeThroughOccluderCheckIndividualObjects)
            {
                for (var r = 0; r < rms.Length; r++)
                    if (rms[r].renderer != null)
                    {
                        var bounds = rms[r].renderer.bounds;
                        var pos = bounds.center;
                        var maxDistance = Vector3.Distance(pos, camPos);
                        if (Physics.BoxCast(pos, bounds.extents * seeThroughOccluderThreshold,
                                (camPos - pos).normalized, out var hitInfo, Quaternion.identity, maxDistance,
                                seeThroughOccluderMask))
                        {
                            occlusionData.cachedOccluderCollider = hitInfo.collider;
                            occlusionData.lastOcclusionTestResult = true;
                            return true;
                        }
                    }

                occlusionData.lastOcclusionTestResult = false;
                return false;
            }
            else
            {
                // Compute combined bounds
                var bounds = rms[0].renderer.bounds;
                for (var r = 1; r < rms.Length; r++)
                    if (rms[r].renderer != null)
                        bounds.Encapsulate(rms[r].renderer.bounds);

                var pos = bounds.center;
                var maxDistance = Vector3.Distance(pos, camPos);
                occlusionData.lastOcclusionTestResult = Physics.BoxCast(pos,
                    bounds.extents * seeThroughOccluderThreshold, (camPos - pos).normalized, out var hitInfo,
                    Quaternion.identity, maxDistance, seeThroughOccluderMask);
                occlusionData.cachedOccluderCollider = hitInfo.collider;
                return occlusionData.lastOcclusionTestResult;
            }
        }


        private const int MAX_OCCLUDER_HITS = 50;
        private static RaycastHit[] occluderHits;

        private void AddWithoutRepetition(List<Renderer> target, List<Renderer> source)
        {
            var sourceCount = source.Count;
            for (var k = 0; k < sourceCount; k++)
            {
                var entry = source[k];
                if (entry != null && !target.Contains(entry) && ValidRenderer(entry)) target.Add(entry);
            }
        }

        private void CheckOcclusionAccurate(CommandBuffer cbuf, Camera cam)
        {
            if (!perCameraOcclusionData.TryGetValue(cam, out var occlusionData))
            {
                occlusionData = new PerCameraOcclusionData();
                perCameraOcclusionData[cam] = occlusionData;
            }

            var now = GetTime();
            var frameCount = Time.frameCount; // ensure all cameras are checked this frame
            var reuse = now - occlusionData.checkLastTime < seeThroughOccluderCheckInterval && Application.isPlaying &&
                        occlusionData.occlusionRenderFrame != frameCount;

            if (!reuse)
            {
                occlusionData.cachedOccluders.Clear();
                occlusionData.cachedOccluderCollider = null;

                if (rms == null || rms.Length == 0 || rms[0].renderer == null) return;

                occlusionData.checkLastTime = now;
                occlusionData.occlusionRenderFrame = frameCount;
                var quaternionIdentity = Quaternion.identity;
                var camPos = cam.transform.position;

                if (occluderHits == null || occluderHits.Length < MAX_OCCLUDER_HITS)
                    occluderHits = new RaycastHit[MAX_OCCLUDER_HITS];

                if (seeThroughOccluderCheckIndividualObjects)
                {
                    for (var r = 0; r < rms.Length; r++)
                        if (rms[r].renderer != null)
                        {
                            var bounds = rms[r].renderer.bounds;
                            var pos = bounds.center;
                            var maxDistance = Vector3.Distance(pos, camPos);
                            var numOccluderHits = Physics.BoxCastNonAlloc(pos,
                                bounds.extents * seeThroughOccluderThreshold, (camPos - pos).normalized, occluderHits,
                                quaternionIdentity, maxDistance, seeThroughOccluderMask);
                            for (var k = 0; k < numOccluderHits; k++)
                            {
                                occluderHits[k].collider.transform.root.GetComponentsInChildren(tempRR);
                                AddWithoutRepetition(occlusionData.cachedOccluders, tempRR);
                            }
                        }
                }
                else
                {
                    // Compute combined bounds
                    var bounds = rms[0].renderer.bounds;
                    for (var r = 1; r < rms.Length; r++)
                        if (rms[r].renderer != null)
                            bounds.Encapsulate(rms[r].renderer.bounds);

                    var pos = bounds.center;
                    var maxDistance = Vector3.Distance(pos, camPos);
                    var numOccluderHits = Physics.BoxCastNonAlloc(pos, bounds.extents * seeThroughOccluderThreshold,
                        (camPos - pos).normalized, occluderHits, quaternionIdentity, maxDistance,
                        seeThroughOccluderMask);
                    for (var k = 0; k < numOccluderHits; k++)
                    {
                        occluderHits[k].collider.transform.root.GetComponentsInChildren(tempRR);
                        AddWithoutRepetition(occlusionData.cachedOccluders, tempRR);
                    }
                }
            }

            // render occluders
            var occluderRenderersCount = occlusionData.cachedOccluders.Count;
            if (occluderRenderersCount > 0)
                for (var k = 0; k < occluderRenderersCount; k++)
                {
                    var r = occlusionData.cachedOccluders[k];
                    if (r != null) cbuf.DrawRenderer(r, fxMatSeeThroughMask);
                }
        }

        public void GetOccluders(Camera camera, List<Transform> occluders)
        {
            occluders.Clear();
            if (perCameraOcclusionData.TryGetValue(camera, out var occlusionData))
            {
                if (occlusionData.cachedOccluderCollider != null)
                {
                    occluders.Add(occlusionData.cachedOccluderCollider.transform);
                    return;
                }

                foreach (var r in occlusionData.cachedOccluders)
                    if (r != null)
                        occluders.Add(r.transform);
            }
        }
    }
}