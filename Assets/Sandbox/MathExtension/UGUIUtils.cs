using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using UnityEngine.UI;

namespace Experimental
{
    /// <summary>
    /// Utility class for UGUI Components.
    /// </summary>
    public sealed class UGUIUtils
    {
        /// <summary>
        /// Initialize a dropdown with enum values.
        /// </summary>
        public static void DropdownWithEnum<T>(Dropdown dropdown) where T : struct, Enum
        {
            var enumList = Enum.GetValues(typeof(T)).Cast<T>();
            var optionList = (from x in enumList select new Dropdown.OptionData(x.ToString())).ToList();
            dropdown.AddOptions(optionList);
        }
    }
}