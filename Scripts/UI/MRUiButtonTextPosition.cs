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
                resizeRepositonText(.03f, 130);
            }
            else
            {
                resizeRepositonText(0, 200);
            }
        }

        private void resizeRepositonText(float xPos, int width)
        {
            if (text == null || text.canvas == null)
            {
                return;
            }
            RectTransform rt = text.canvas.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(xPos, 0, -.011f);
            rt.sizeDelta = new Vector2(width, 80);

            rt = text.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2((width * 2) - 6, 140 - 6);
        }
    }
}