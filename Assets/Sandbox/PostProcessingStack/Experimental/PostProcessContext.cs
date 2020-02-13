using UnityEngine;
using UnityEngine.Rendering;

namespace Experimental.PostProcessStack
{
    public class PostProcessContext
    {
        public Camera Camera
        {
            get;
            set;
        }

        public RenderTexture Source
        {
            get;
            set;
        }

        public RenderTexture Destination
        {
            get;
            set;
        }

        public SheetContainer Sheets
        {
            get;
            set;
        }


        public void Reset()
        {
            Camera = null;

            Source = null;
            Destination = null;

            Sheets = null;

        }



    }
}