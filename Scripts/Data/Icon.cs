using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRUI
{
    [Serializable]
    public class Icon
    {
        public string name;
        public GameObject icon;
        public Vector3 iconScale = new Vector3(.04f, .04f, 1);
    }
}