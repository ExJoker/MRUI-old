using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// data defying button properties (title and icon)
/// </summary>
namespace MRUi
{
    [System.Serializable]
    public class MRUiButtonData
    {
        [Tooltip("Title text shown in the button")]
        public string title = "Button";

        [Tooltip("Icon (normally just a Sprite in a GameObject but you can use any 3D-Object)")]
        public GameObject icon;

        [Tooltip("Additional button data (e.g. some object id)")]
        public string data = null;

        [Tooltip("defines if the button is selected (for toggle-buttons)")]
        public bool selected = false;

        [Tooltip("Use disabled-material and ignore selection")]
        public bool active = true;

        public MRUiButtonData copy()
        {
            return new MRUiButtonData()
            {
                title = title,
                icon = icon,
                data = data,
                selected = selected,
                active = active
            };
        }

        public bool compare(MRUiButtonData other)
        {
            return (title != other.title ||
                icon != other.icon ||
                data != other.data ||
                selected != other.selected ||
                active != other.active);
        }
    }
}
