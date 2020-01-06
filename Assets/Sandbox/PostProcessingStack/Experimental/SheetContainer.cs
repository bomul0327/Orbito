using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Experimental.PostProcessStack
{
    public class SheetContainer
    {
        private Dictionary<Shader, PropertySheet> sheetDict = new Dictionary<Shader, PropertySheet>();

        public PropertySheet GetOrCreateSheet(string shaderPath)
        {

            var shader = Shader.Find(shaderPath);
            if (shader == null)
                throw new ArgumentException($"Failed to find shader at '{shaderPath}'.");

            return GetOrCreateSheet(shader);
        }

        public PropertySheet GetOrCreateSheet(Shader shader)
        {
            PropertySheet sheet;

            if (sheetDict.TryGetValue(shader, out sheet))
            {
                return sheet;
            }

            //If sheet for the shader does not exist, create and add new one.
            var newMaterial = new Material(shader);
            newMaterial.hideFlags = HideFlags.DontSave;

            var newSheet = new PropertySheet(newMaterial);

            sheetDict.Add(shader, newSheet);

            return newSheet;

        }
    }
}