using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRUi
{
    [ExecuteInEditMode]
    public class MRUiButtonIcon : MonoBehaviour
    {
        public UnityEvent OnIconChange;

        [HideInInspector]
        public GameObject icon;

        public string ICON_NAME = "icon";

        public void updateData()
        {
            MRUiButton btn = GetComponent<MRUiButton>();
            destroyIcon();

            if (btn.data != null && btn.data.icon != null)
            {
                icon = Instantiate(btn.data.icon, transform);
                icon.name = ICON_NAME;
                OnIconChange.Invoke();
            }
        }

        private void OnDestroy()
        {
            destroyIcon();
        }

        private void destroyIcon()
        {
            if (this != null && transform != null)
            {
                foreach (Transform child in transform)
                {
                    if (child.name == ICON_NAME)
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }
    }

}