using UnityEngine;

namespace Experimental.PostProcessStack
{
    [CreateAssetMenu(fileName = "New GrayscaleEffect", menuName = "PostProcessing/New GrayScale Effect")]
    public class GrayscalePostProcessEffect : PostProcessEffectBase
    {
        [Range(0, 1)] public float amount;

        public override void Render(PostProcessContext context)
        {
            var sheet = context.Sheets.GetOrCreateSheet("Hidden/PostProcessing/Grayscale");
            sheet.Material.SetFloat(Grayscale_Amount_ID, amount);

            Graphics.Blit(context.Source, context.Destination, sheet.Material);               
        }

        public override bool IsEnabled()
        {
            return amount > 0;
        }

        /// Shader Property IDs
        private static readonly int Grayscale_Amount_ID = Shader.PropertyToID("_Amount");
    }
} 