using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// position the icon
/// </summary>
namespace MRUI
{
    [RequireComponent(typeof(MRUI.Button))]
    [RequireComponent(typeof(ButtonIcon))]

    [ExecuteInEditMode]
    public class ButtonIconPosition : MonoBehaviour
    {
        [Tooltip("Position the icon will be moved to, when it is set. Use with the MRUiButtonIcon-Script")]
        public Vector3 fixPosition = new Vector3(-0.06f, 0, -0.011f);

        [Tooltip("Scale for the icon")]
        public Vector3 fixScale = new Vector3(0.03f, 0.03f, 1f);

        public void OnEnable()
        {
            AddEvents();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UpdateIcon();
            };
#else
            UpdateIcon();
#endif
        }

        public void OnDisable()
        {
            RemoveEvents();
        }

        public void AddEvents()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            btnIcon.OnIconChanged.AddListener(UpdateIcon);
        }

        public void RemoveEvents()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            if (btnIcon != null)
            {
                btnIcon.OnIconChanged.RemoveListener(UpdateIcon);
            }
        }

        public void UpdateIcon()
        {
            if (this == null)
            {
                return;
            }
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            if (btnIcon == null || btnIcon.icon == null)
            {
                return;
            }
            Button btn = GetComponent<MRUI.Button>();
            if (btn != null && btn.data != null)
            {
                btnIcon.icon.transform.localScale = btn.data.iconScale;
            }
            else
            {
                btnIcon.icon.transform.localScale = fixScale;
            }
            btnIcon.icon.transform.localPosition = fixPosition;
        }
    }
}