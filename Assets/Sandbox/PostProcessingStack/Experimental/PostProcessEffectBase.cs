using UnityEngine;

namespace Experimental.PostProcessStack
{
    [System.Serializable]
    public abstract class PostProcessEffectBase
    {
        public abstract void Render(PostProcessContext context);

        public abstract bool IsDisabled();
    }
}