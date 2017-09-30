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

        private bool eventsAdded = false;

        public void Start()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            btnIcon.OnIconChange.AddListener(updateIcon);
            updateIcon();
        }

        private void OnDestroy()
        {
            if (!eventsAdded)
            {
                return;
            }
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            if (btnIcon != null)
            {
                btnIcon.OnIconChange.RemoveListener(updateIcon);
            }
            eventsAdded = false;
        }

#if UNITY_EDITOR
        public void OnEnable()
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this == null)
                {
                    return;
                }
                updateIcon();
            };
        }

        public void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this == null)
                {
                    return;
                }
                updateIcon();
            };
        }
#endif

        private void updateIcon()
        {
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