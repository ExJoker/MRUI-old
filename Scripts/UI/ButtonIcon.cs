using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRUI
{
    [RequireComponent(typeof(MRUI.Button))]

    [ExecuteInEditMode]
    public class ButtonIcon : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent OnIconChange;

        [HideInInspector]
        public GameObject icon;

        public string ICON_NAME = "icon";

        public void Awake()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            btn.OnDataChanged.AddListener(updateData);
        }

        public void updateData()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
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
            MRUI.Button btn = GetComponent<MRUI.Button>();
            if (btn != null)
            {
                btn.OnDataChanged.RemoveListener(updateData);
            }
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