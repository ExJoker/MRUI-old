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
        public void OnEnable()
        {
            AddEvents();
            updateData();
        }

        public void OnDisable()
        {
            RemoveEvents();
        }

        public void AddEvents()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            btn.OnDataChanged.AddListener(updateData);
        }

        public void RemoveEvents()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            if (btn != null)
            {
                btn.OnDataChanged.RemoveListener(updateData);
            }
        }

        public void updateData()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            Text txt = GetComponentInChildren<Text>();
            txt.text = btn.data.title;
            
            // workaround: disable and re-enable to force redraw
            txt.enabled = !txt.enabled;
            txt.enabled = !txt.enabled;
        }
    }
}
