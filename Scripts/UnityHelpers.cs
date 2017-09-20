using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MRUi
{
    public class UnityHelpers
    {
        /// <summary>
        /// recursively go through given element and all its parents until a component is found.
        /// </summary>
        public static T GetComponentInAllParents<T>(GameObject go)
        {
            T child = go.GetComponent<T>();
            if (child == null && go.transform.parent != null)
            {
                return GetComponentInAllParents<T>(go.transform.parent.gameObject);
            }
            return child;
        }
    }
}