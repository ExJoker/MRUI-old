using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// change text content when button data changes
/// </summary>
namespace MRUI
{
    [RequireComponent(typeof(MRUI.Button))]

    [ExecuteInEditMode]
    public class ButtonText : MonoBehaviour
    {
#if UNITY_EDITOR
        private bool forceUpdate = false;
#endif

        public void Awake()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            btn.OnDataChanged.AddListener(updateData);
        }

        public void OnDestroy()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            if (btn != null)
            {
                btn.OnDataChanged.RemoveListener(updateData);
            }
        }

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
            MRUI.Button btn = GetComponent<MRUI.Button>();
#if UNITY_EDITOR
            forceUpdate = false;
#endif
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
#endif
    }
}
