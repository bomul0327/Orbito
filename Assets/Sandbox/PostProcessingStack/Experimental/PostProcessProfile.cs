using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experimental.PostProcessStack
{
    [System.Serializable]
    public class PostProcessProfile
    {
        [SerializeField] List<PostProcessEffectBase> effectList = new List<PostProcessEffectBase>();

        public List<PostProcessEffectBase> EffectList
        {
            get => effectList;
        }

    }
}