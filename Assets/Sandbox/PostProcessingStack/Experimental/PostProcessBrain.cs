using UnityEngine;
using UnityEngine.Rendering;

namespace Experimental.PostProcessStack
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class PostProcessBrain : MonoBehaviour
    {
        [SerializeField] PostProcessProfile profile;

        private PostProcessContext currentContext;
        private CommandBuffer postProcessingBuffer;

        private SheetContainer sheetContainer;

        static readonly int cameraColorTextureID = Shader.PropertyToID("_CameraColorTexture");
        static readonly int cameraDepthTextureID = Shader.PropertyToID("_CameraDepthTexture");

        public Camera Camera
        {
            get;
            private set;
        }


        private void OnEnable()
        {
            Camera = GetComponent<Camera>();
            currentContext = new PostProcessContext();
            sheetContainer = new SheetContainer();
            postProcessingBuffer = new CommandBuffer()
            {
                name = "Post Processing"
            };
            Camera.AddCommandBuffer(CameraEvent.AfterImageEffects, postProcessingBuffer);
            BuildContext();
        }

        private void OnDisable()
        {
            currentContext.Reset();
            sheetContainer = null;
            Camera.RemoveCommandBuffer(CameraEvent.AfterImageEffects, postProcessingBuffer);
            postProcessingBuffer.Release();

        }


        private void OnPreRender()
        {
            BuildContext();

        }


        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination);



        }
        /// <summary>
        /// FixMe
        /// </summary>
        private void OnPostRender()
        {
            //currentContext.Source = source;
            //currentContext.Destination = destination;

            postProcessingBuffer.GetTemporaryRT(cameraColorTextureID, Camera.pixelWidth, Camera.pixelHeight, 0);
            postProcessingBuffer.GetTemporaryRT(cameraDepthTextureID, Camera.pixelWidth, Camera.pixelHeight, 24, FilterMode.Point, RenderTextureFormat.Depth);

            //currentContext.Command.Blit(currentContext.Source, currentContext.Destination);
            Graphics.ExecuteCommandBuffer(postProcessingBuffer);
            Render();

            postProcessingBuffer.Clear();
            postProcessingBuffer.SetRenderTarget(
                cameraColorTextureID, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                cameraDepthTextureID, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store
            );

            Graphics.ExecuteCommandBuffer(postProcessingBuffer);
            postProcessingBuffer.Clear();

            postProcessingBuffer.ReleaseTemporaryRT(cameraColorTextureID);
            postProcessingBuffer.ReleaseTemporaryRT(cameraDepthTextureID);
        }

        private void Render()
        {
            var effects = profile.EffectList;

            foreach (var effect in effects)
            {
                ProcessEffect(effect, currentContext);
            }
        }

        private void BuildContext()
        {
            var context = currentContext;
            context.Reset();

            context.Sheets = sheetContainer;
            context.Camera = Camera;
            context.Command = postProcessingBuffer;
        }


        private void ProcessEffect(PostProcessEffectBase effect, PostProcessContext context)
        {
            effect.Render(context);
        }
    }
}