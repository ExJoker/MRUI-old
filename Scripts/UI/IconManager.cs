using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRUI
{

    public class IconManager : MonoBehaviour
    {
        public List<Icon> icons;

        public GameObject iconByName(string name)
        {
            foreach (Icon icon in icons)
            {
                if (icon.name == name)
                {
                    return icon.icon;
                }
            }
            return null;
        }

        public Vector3? iconScaleByName(string name)
        {
            foreach (Icon icon in icons)
            {
                if (icon.name == name)
                {
                    return icon.iconScale;
                }
            }
            return null;
        }
    }
}