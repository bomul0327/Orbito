using UnityEngine;

namespace Experimental.PostProcessStack
{
    [CreateAssetMenu(fileName = "New ColorGrading Effect", menuName = "PostProcessing/New ColorGrading Effect")]
    public class ColorGradingPostProcessEffect : PostProcessEffectBase
    {
        public Color tintColor;

        public Texture2D gradingTexture;

        public override void Render(PostProcessContext context)
        {
            var sheet = context.Sheets.GetOrCreateSheet("Hidden/PostProcessing/ColorGrading");

            sheet.Material.SetColor(TintColorID, tintColor);

            Graphics.Blit(context.Source, context.Destination, sheet.Material);               
        }

        public override bool IsEnabled()
        {
            return true;
        }

        /// Shader Property IDs
        private static readonly int TintColorID = Shader.PropertyToID("_TintColor");
    }
} 