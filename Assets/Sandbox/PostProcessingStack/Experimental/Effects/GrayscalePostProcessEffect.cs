using UnityEngine;

namespace Experimental.PostProcessStack
{
    public class GrayscalePostProcessEffect : PostProcessEffectBase
    {
        [Range(0, 1)] public float amount;

        public override void Render(PostProcessContext context)
        {
            if (IsDisabled())
                return;

            var sheet = context.Sheets.GetOrCreateSheet("PostProcessing/Grayscale");
            sheet.Properties.SetFloat(Grayscale_Amount_ID, amount);
            context.Command.BlitFullscreenTriangle(context.Source, context.Destination, 0, false, sheet);
        }

        public override bool IsDisabled()
        {
            return amount == 0;
        }

        /// Shader Property IDs
        private static readonly int Grayscale_Amount_ID = Shader.PropertyToID("_Amount");
    }
}