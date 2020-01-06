using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrayscaleRenderer : MonoBehaviour
{
    [Range(0, 1)] public float amount = 1;

    private Material material;
    private static readonly string shaderPath = "Hidden/PostProcessing/Grayscale";
    Material Material
    {
        get
        {
            if (material)
                return material;

            var shader = Shader.Find(shaderPath);
            if (shader == null)
                throw new System.ArgumentException($"Could not find {shaderPath}");

            material = new Material(shader);
            material.hideFlags = HideFlags.HideAndDontSave;

            return material;
        }
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Material.SetFloat("_Amount", amount);
        Graphics.Blit(source, destination, material);
    }


}
