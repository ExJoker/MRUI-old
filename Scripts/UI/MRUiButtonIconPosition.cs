using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// position the icon
/// </summary>
namespace MRUi
{
    [ExecuteInEditMode]
    public class MRUiButtonIconPosition : MonoBehaviour
    {
        [Tooltip("Position the icon will be moved to, when it is set. Use with the MRUiButtonIcon-Script")]
        public Vector3 fixPosition = new Vector3(-0.06f, 0, -0.011f);

        [Tooltip("Scale for the icon")]
        public Vector3 fixScale = new Vector3(0.03f, 0.03f, 1f);

#if UNITY_EDITOR
        private bool forceUpdate;
#endif

        public void Start()
        {
            updateIcon();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            updateIcon();
        }

        private void Update()
        {
            if (forceUpdate)
            {
                _updateIcon();
                forceUpdate = false;
            }
        }
#endif

        private void _updateIcon()
        {
            GameObject icon = GetComponent<MRUiButtonIcon>().icon;
            if (icon != null)
            {
                icon.transform.localPosition = fixPosition;
                icon.transform.localScale = fixScale;
            }
        }

        // Use this for initialization
        public void updateIcon()
        {
#if UNITY_EDITOR
            forceUpdate = true;
#else
            _updateIcon();
#endif
        }
    }
}