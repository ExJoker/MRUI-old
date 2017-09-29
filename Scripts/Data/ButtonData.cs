using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// data defying button properties (title and icon)
/// </summary>
namespace MRUI
{
    [System.Serializable]
    public struct ButtonMaterial
    {
        public Material normal;
        public Material highlighted;
        public Material pressed;
    }

    [System.Serializable]
    public class ButtonData
    {
        [Tooltip("Title text shown in the button")]
        public string title = "Button";

        [Tooltip("Icon (normally just a Sprite in a GameObject but you can use any 3D-Object)")]
        public GameObject icon;

        [Tooltip("Scale of icon")]
        public Vector3 iconScale = new Vector3(.04f, .04f, 1f);

        [Tooltip("Additional button data (e.g. some object id)")]
        public string data = null;

        [Tooltip("defines if the button is selected (for toggle-buttons)")]
        public bool selected = false;

        [Tooltip("Use disabled-material and ignore selection")]
        public bool active = true;

        public ButtonMaterial material;

        public ButtonData copy()
        {
            return new ButtonData()
            {
                title = title,
                icon = icon,
                data = data,
                selected = selected,
                active = active,
                iconScale = iconScale,
                material = material
            };
        }

        /// <summary>
        /// compare data of two ButtonData, returns true if it is the same.
        /// (ignores purely optical attributes: material and scale)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool compare(ButtonData other)
        {
            return (title != other.title ||
                icon != other.icon ||
                data != other.data ||
                selected != other.selected ||
                active != other.active); 
            //note that we ignore the material and scale because the core data should still be the same.
        }
    }
}
