using UnityEngine;

namespace Experimental.PostProcessStack
{
    [System.Serializable]
    public abstract class PostProcessEffectBase : ScriptableObject
    {
        public abstract void Render(PostProcessContext context);

        public abstract bool IsEnabled();
    }
}