using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// change text content when button data changes
/// </summary>
namespace MRUi
{
    [ExecuteInEditMode]
    public class MRUiButtonText : MonoBehaviour
    {
#if UNITY_EDITOR
        private bool forceUpdate = false;
#endif

        public void updateData()
        {
            // update text next frame - we setting the text directly does not work in Editor!
#if UNITY_EDITOR
            forceUpdate = true;
#else
            UpdateText();
#endif
        }

        private void UpdateText()
        {
            MRUiButton btn = GetComponent<MRUiButton>();
            forceUpdate = false;
            GetComponentInChildren<Text>().text = btn.data.title;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (forceUpdate)
            {
                UpdateText();
            }
        }
    }
#endif
}
