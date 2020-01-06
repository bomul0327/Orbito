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

        public static Mesh FullscreenTriangleMesh
        {
            get
            {
                if (fullscreenTriangleMesh)
                    return fullscreenTriangleMesh;

                fullscreenTriangleMesh = new Mesh();

                ///
                /// This is a well-known trick that is used for drawing fullscreen triangle.
                /// By using this trick and VertexID, we can easily calculate the coordinate
                /// without creating any buffers or sending any data to the shader.
                /// 
                /// Unity Post Processing Stack V2 internally uses this trick.
                /// 
                /// <see cref="https://www.reddit.com/r/gamedev/comments/2j17wk/a_slightly_faster_bufferless_vertex_shader_trick/"/>
                ///
                var vertices = new List<Vector3>() { new Vector3(-1, -1, 0), new Vector3(-1, 3, 0), new Vector3(3, -1, 0) };
                fullscreenTriangleMesh.SetVertices(vertices);

                var indices = new int[] { 0, 1, 2 };
                fullscreenTriangleMesh.SetIndices(indices, MeshTopology.Triangles, 0, false);

                fullscreenTriangleMesh.UploadMeshData(false);

                return fullscreenTriangleMesh;
            }

        }


        public static void SetRenderTargetWithActions(this CommandBuffer command, RenderTargetIdentifier rt, RenderBufferLoadAction loadAction, RenderBufferStoreAction storeAction)
        {
            command.SetRenderTarget(rt, loadAction, storeAction);
        }

        public static void SetRenderTargetWithActions(this CommandBuffer command, RenderTargetIdentifier colorRT, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderTargetIdentifier depthRT, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
        {
            command.SetRenderTarget(colorRT, colorLoadAction, colorStoreAction, depthRT, depthLoadAction, depthStoreAction);
        }

        public static void BlitFullscreenTriangle(this CommandBuffer command, RenderTargetIdentifier source, RenderTargetIdentifier destination, bool clearDestination)
        {
            command.SetGlobalTexture(MainTexID, source);
            command.SetRenderTarget(destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);

            if (clearDestination)
                command.ClearRenderTarget(true, true, Color.clear);

            command.DrawMesh(FullscreenTriangleMesh, Matrix4x4.identity, CopyMaterial, 0, 0);
        }

        public static void BlitFullscreenTriangle(this CommandBuffer command, RenderTargetIdentifier source, RenderTargetIdentifier destination, int pass, bool clearDestination = false, PropertySheet propertySheet = null)
        {
            command.SetGlobalTexture(MainTexID, source);
            command.SetRenderTargetWithActions(destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            if (clearDestination)
                command.ClearRenderTarget(true, true, Color.clear);
            if (propertySheet == null)
            {
                command.BlitFullscreenTriangle(source, destination, pass, clearDestination);
                return;
            }
            command.DrawMesh(FullscreenTriangleMesh, Matrix4x4.identity, propertySheet.Material, 0, pass, propertySheet.Properties);
        }

    }
}