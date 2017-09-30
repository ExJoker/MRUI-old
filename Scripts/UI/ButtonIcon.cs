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
        public UnityEvent OnIconChanged;

        [HideInInspector]
        public GameObject icon;

        public string ICON_NAME = "icon";

        private void Awake()
        {
            if (OnIconChanged == null)
                OnIconChanged = new UnityEvent(); 
        }

        public void OnEnable()
        {
            AddEvents();
            UpdateData();
        }

        public void OnDisable()
        {
            RemoveEvents();
        }

        private void OnDestroy()
        {
            // Note that we do not need to call RemoveEvents, because
            // OnDisabled is called by Destroy automatically
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyIcon();
            };
#else
            DestroyIcon();
#endif
        }

        public void UpdateData()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this == null)
                {
                    return;
                }
                DestroyIcon();
                CreateIcon();
            };
#else
            DestroyIcon();
            CreateIcon();
#endif
        }

        private void CreateIcon()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            if (btn.data != null && btn.data.icon != null)
            {
                icon = Instantiate(btn.data.icon, transform);
                icon.name = ICON_NAME;
                OnIconChanged.Invoke();
            }
        }

        public void AddEvents()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            btn.OnDataChanged.AddListener(UpdateData);
        }

        public void RemoveEvents()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            if (btn != null)
            {
                btn.OnDataChanged.RemoveListener(UpdateData);
            }
        }

        private void DestroyIcon()
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