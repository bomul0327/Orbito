using UnityEngine;
using UnityEngine.Rendering;

namespace Experimental.PostProcessStack
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostProcessBrain : MonoBehaviour
    {
        [SerializeField] PostProcessProfile profile;

        private PostProcessContext currentContext;

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

            BuildContext();
        }

        private void OnDisable()
        {
            currentContext.Reset();
            sheetContainer = null;

        }


        private void OnPreRender()
        {
            BuildContext();

        }


        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            currentContext.Source = source;
            currentContext.Destination = destination;

            RenderEffects();
        }



        private void RenderEffects()
        {
            var effects = profile.EffectList;

            //If there are no effects to render, just blit(source -> dest).
            if (effects == null || effects.Count == 0)
            {
                Graphics.Blit(currentContext.Source, currentContext.Destination);
                return;
            }

            //Render each effects and blit (dest -> source) for switching.
            for (int i = 0; i < effects.Count; i++)
            {
                ProcessEffect(effects[i], currentContext);

                //Note that we don't need to blit (dest -> source) for the last effect.
                if (i < effects.Count - 1 && effects[i].IsEnabled())
                {
                    Graphics.Blit(currentContext.Destination, currentContext.Source);
                }
            }
        }

        /// <summary>
        /// Build & set up PostProcessContext
        /// </summary>
        private void BuildContext()
        {
            var context = currentContext;
            context.Reset();
            
            context.Sheets = sheetContainer;
            context.Camera = Camera;
        }

        /// <summary>
        /// Process a effect
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="context"></param>
        private void ProcessEffect(PostProcessEffectBase effect, PostProcessContext context)
        {
            effect.Render(context);
        }
    }
}