using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experimental.PostProcessStack
{
    [CreateAssetMenu(fileName = "New PostProcessing Profile", menuName = "PostProcessing/New PostProcessing Profile")]
    public class PostProcessProfile : ScriptableObject
    {
        [SerializeField] List<PostProcessEffectBase> effectList = new List<PostProcessEffectBase>();

        public List<PostProcessEffectBase> EffectList
        {
            get => effectList;
        }

    }
}