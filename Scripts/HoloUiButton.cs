using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HoloUi
{
    public class HoloUiButton : MonoBehaviour
    {
        public HoloUiButtonData data;
        [Tooltip("calculate Text and Image position. Set to false if you'd like to manipulate it manually.")]
        public bool dataDefinesSizeAndPosition = true;

        public enum Transition { None, Material };
        public Transition transition;

        public Material normalMaterial;
        public Material highlightedMaterial;
        public Material pressedMaterial;

        public float fadeDuration = .1f;

        [Tooltip("defines if this should behave like a toggle button")]
        public bool toggle;

        [Tooltip("Tapped-event (air tap or clicker, simulated by touch or left click)")]
        public UnityEvent OnPressed;

        [Tooltip("Gaze on Button (hover with mouse).")]
        public UnityEvent OnHighlighted;

        [Tooltip("Rednerer that we will assign the material to. If this is null we will try to get the Renderer from the component.")]
        public Renderer rend;

        public Text text;

        private GameObject oldIcon;
        private bool oldDataDefinesSizeAndPosition;

        void Start()
        {
            if (rend == null)
            {
                rend = GetComponent<Renderer>();
            }
            if (normalMaterial != null && rend != null)
            {
                rend.material = normalMaterial;
            }
            if (text == null)
            {
                text = GetComponentInChildren<Text>();
            }
            updateData();
        }

        private void destroyIcon()
        {
            if (this != null && transform != null)
            {
                foreach (Transform child in transform)
                {
                    if (child.name == "icon")
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        private void resizeText(float xPos, int width)
        {
            if (text == null || text.canvas == null)
            {
                return;
            }
            RectTransform rt = text.canvas.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(xPos, 0, -.1f);
            rt.sizeDelta = new Vector2(width, 65);

            rt = text.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2((width * 2) - 6, 130 - 6);
        }

#if UNITY_EDITOR
        private void updateIconDelayed()
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                updateIcon();
            };
        }
#endif

        private void updateIcon()
        {
            if (this == null)
            {
                return;
            }
            destroyIcon();
            if (data.icon != null)
            {
                GameObject iconInstance = Instantiate(data.icon, transform);
                iconInstance.name = "icon";
                if (dataDefinesSizeAndPosition)
                {
                    iconInstance.transform.localPosition = new Vector3(-2, 0, 0);
                    resizeText(1f, 100);
                }
            }
            else if (dataDefinesSizeAndPosition)
            {
                resizeText(0, 150);
            }
            oldDataDefinesSizeAndPosition = dataDefinesSizeAndPosition;
            oldIcon = data.icon;
        }

        public void updateData()
        {
            if (oldIcon != data.icon || dataDefinesSizeAndPosition != oldDataDefinesSizeAndPosition)
            {
#if UNITY_EDITOR
                updateIconDelayed();
#else
                updateIcon();
#endif
            }
            

            if (text == null || text.text == data.title)
            {
                return;
            }
            text.text = data.title;
        }

        private void OnDestroy()
        {
            destroyIcon();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            updateData();
        }
#endif

    }
}