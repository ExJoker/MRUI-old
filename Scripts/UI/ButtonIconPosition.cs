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

        public void Start()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            btnIcon.OnIconChange.AddListener(updateIcon);
            updateIcon();
        }

        private void OnDestroy()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            if (btnIcon != null)
            {
                btnIcon.OnIconChange.RemoveListener(updateIcon);
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            updateIcon();
        }
#endif

        private void updateIcon()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            if (btnIcon == null || btnIcon.icon == null)
            {
                return;
            }
            btnIcon.icon.transform.localPosition = fixPosition;
            btnIcon.icon.transform.localScale = fixScale;
        }
    }
}