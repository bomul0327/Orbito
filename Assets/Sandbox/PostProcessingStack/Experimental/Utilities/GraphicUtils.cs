using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

namespace Experimental.PostProcessStack
{
    public static class GraphicUtils
    {
        //Pre-hashed ids of "_MainTex" property
        static readonly int MainTexID = Shader.PropertyToID("_MainTex");

        static Mesh fullscreenTriangleMesh;
        static Material copyMaterial;

        static Material CopyMaterial
        {
            get
            {
                if (copyMaterial)
                    return copyMaterial;

                var shader = Shader.Find("Hidden/PostProcessing/Copy");
                if (shader == null)
                    throw new System.ArgumentException($"Could not find {"Hidden/PostProcessing/Copy"}");

                copyMaterial = new Material(shader);
                copyMaterial.hideFlags = HideFlags.HideAndDontSave;

                return copyMaterial;
            }
        }

    }
}