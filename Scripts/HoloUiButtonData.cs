using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// data defying button properties (title and icon)
/// </summary>
namespace HoloUi
{
    [System.Serializable]
    public class HoloUiButtonData
    {
        [Tooltip("Title text shown in the button")]
        public string title = "Button";

        [Tooltip("Icon (normally just a Sprite in a GameObject but you can use any 3D-Object)")]
        public GameObject icon;

        [Tooltip("Additional button data")]
        public Object data = null;

        [Tooltip("defines if the button is selected (for toggle-buttons)")]
        public bool selected = false;

        [Tooltip("Use disabled-material and ignore selection")]
        public bool active = true;
    }
}
