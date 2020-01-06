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

        public RenderTargetIdentifier Source
        {
            get;
            set;
        }

        public RenderTargetIdentifier Destination
        {
            get;
            set;
        }

        public CommandBuffer Command
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
            Source = 0;
            Destination = 0;
            Sheets = null;
            Command = null;

        }



    }
}