using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experimental.PostProcessStack
{
    public class PropertySheet
    {
        public Material Material
        {
            get;
            set;
        }

        public MaterialPropertyBlock Properties
        {
            get;
            set;
        }

        public PropertySheet(Material material)
        {
            Material = material;
            Properties = new MaterialPropertyBlock();

        }

        public void Release()
        {
            Properties.Clear();
        }

    }
}