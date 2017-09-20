using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// A button always consists of a text field and a background image. 
/// It can also show an icon (some gameobject).
/// The icon and the text content are stored in the given ButtonData
/// </summary>
namespace MRUi
{
    public class MRUiButton : MonoBehaviour
    {
        public MRUiButtonData data;
        private MRUiButtonData oldData;

        [Tooltip("Calculate text and icon position. Set to true to make space for the icon automatically, Set it to false if you'd like to manipulate it manually.")]
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

        [Tooltip("Data has been changed")]
        public UnityEvent OnDataChanged;

        [Tooltip("Gaze on Button (hover with mouse).")]
        public UnityEvent OnHighlighted;

        // renderer is needed to change the material when highlighted (hovered) or pressed (tabbed)
        [Tooltip("Rednerer that we will assign the material to. If this is null we will try to get the Renderer from the component.")]
        public Renderer rend;
        
        public bool dataChanged()
        {
            return (oldData != data || oldData.icon != data.icon || oldData.title != data.title);
        }

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
            updateData();
        }

        private void updateChanged()
        {
            if (dataChanged() && OnDataChanged != null)
            {
                OnDataChanged.Invoke();
                oldData = data.copy();
            }
        }

        public void updateData()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this != null)
                {
                    updateChanged();
                }
            };
#else
            updateChanged();
#endif
        }
        

#if UNITY_EDITOR
        private void OnValidate()
        {
            updateData();
        }
#endif

    }
}