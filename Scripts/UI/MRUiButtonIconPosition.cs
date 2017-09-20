using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// position the icon
/// </summary>
namespace MRUi
{
    public class MRUiButtonIconPosition : MonoBehaviour
    {
        [Tooltip("Position the icon will be moved to, when it is set. Use with the MRUiButtonIcon-Script")]
        public Vector3 fixPosition = new Vector3(-2, 0, 0);

        public void OnValidate()
        {
            updateIcon();
        }

        // Use this for initialization
        public void updateIcon()
        {
            GameObject icon = GetComponent<MRUiButtonIcon>().icon;
            if (icon != null) {
                icon.transform.localPosition = fixPosition;
            }
        }
    }
}