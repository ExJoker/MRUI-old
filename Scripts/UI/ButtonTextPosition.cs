using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// position text based on icon
/// Made to use in combination with MRUiButton, but can be used without MRUiButton
/// </summary>
namespace MRUI
{
    [RequireComponent(typeof(MRUI.Button))]
    [RequireComponent(typeof(ButtonIcon))]

    [ExecuteInEditMode]
    public class ButtonTextPosition : MonoBehaviour
    {
        private Text text;

        public void OnEnable()
        {
            AddEvents();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UpdateData();
            };
#else
            UpdateData();
#endif
        }

        public void OnDisable()
        {
            RemoveEvents();
        }

        public void AddEvents()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            btnIcon.OnIconChanged.AddListener(UpdatePosition);

            Button btn = GetComponent<Button>();
            btn.OnDataChanged.AddListener(UpdateData);
        }

        public void RemoveEvents()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            if (btnIcon != null)
            {
                btnIcon.OnIconChanged.RemoveListener(UpdatePosition);
            }

            Button btn= GetComponent<Button>();
            if (btn != null)
            {
                btn.OnDataChanged.RemoveListener(UpdateData);
            }
        }

        public void UpdateData()
        {
            if (this == null)
            {
                return;
            }
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            if (text == null)
            {
                text = GetComponentInChildren<Text>();
            }
            if (btn.data != null && btn.data.icon != null)
            {
                ResizeRepositonText(.03f, 130);
            }
            else
            {
                ResizeRepositonText(0, 200);
            }
        }


        private void ResizeRepositonText(float xPos, int width)
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