using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HighlightPlus
{
    public struct MeshData
    {
        public Renderer renderer;
        public int subMeshCount;
    }

    public enum OccluderMode
    {
        BlocksSeeThrough,
        TriggersSeeThrough
    }

    public enum DetectionMethod
    {
        Stencil = 0,
        RayCast = 1
    }

    [ExecuteInEditMode]
    public class HighlightSeeThroughOccluder : MonoBehaviour
    {
        public OccluderMode mode = OccluderMode.BlocksSeeThrough;

        public DetectionMethod detectionMethod = DetectionMethod.Stencil;

        [NonSerialized] public MeshData[] meshData;

        private List<Renderer> rr;

        private void OnEnable()
        {
            if (gameObject.activeInHierarchy) Init();
        }

        private void Init()
        {
            if (mode == OccluderMode.BlocksSeeThrough && detectionMethod == DetectionMethod.RayCast)
            {
                HighlightEffect.RegisterOccluder(this);
                return;
            }

            if (rr == null)
                rr = new List<Renderer>();
            else
                rr.Clear();

            GetComponentsInChildren(rr);
            var rrCount = rr.Count;
            meshData = new MeshData[rrCount];
            for (var k = 0; k < rrCount; k++)
            {
                meshData[k].renderer = rr[k];
                meshData[k].subMeshCount = 1;
                if (rr[k] is MeshRenderer)
                {
                    var mf = rr[k].GetComponent<MeshFilter>();
                    if (mf != null && mf.sharedMesh != null) meshData[k].subMeshCount = mf.sharedMesh.subMeshCount;
                }
                else if (rr[k] is SkinnedMeshRenderer)
                {
                    var smr = (SkinnedMeshRenderer)rr[k];
                    meshData[k].subMeshCount = smr.sharedMesh.subMeshCount;
                }
            }

            if (rrCount > 0) HighlightEffect.RegisterOccluder(this);
        }

        private void OnDisable()
        {
            HighlightEffect.UnregisterOccluder(this);
        }
    }
}