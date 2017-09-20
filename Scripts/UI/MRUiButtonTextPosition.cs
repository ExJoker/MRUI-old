using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// position text based on icon
/// Made to use in combination with MRUiButton, but can be used without MRUiButton
/// </summary>
namespace MRUi
{
    public class MRUiButtonTextPosition : MonoBehaviour
    {
        private Text text;

        public void updateData()
        {
            MRUiButton btn = GetComponent<MRUiButton>();
            if (text == null)
            {
                text = GetComponentInChildren<Text>();
            }
            if (btn.data != null && btn.data.icon != null)
            {
                resizeRepositonText(1f, 100);
            }
            else
            {
                resizeRepositonText(0, 150);
            }
        }

        private void resizeRepositonText(float xPos, int width)
        {
            if (text == null || text.canvas == null)
            {
                return;
            }
            RectTransform rt = text.canvas.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(xPos, 0, -.1f);
            rt.sizeDelta = new Vector2(width, 65);

            rt = text.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2((width * 2) - 6, 130 - 6);
        }
    }
}