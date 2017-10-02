using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A button always consists of a text field and a background image. 
/// It can also show an icon (some gameobject).
/// The icon and the text content are stored in the given ButtonData
/// </summary>
namespace MRUI
{
    [ExecuteInEditMode]
    public class Button : MonoBehaviour
    {
        public ButtonData data;
        private ButtonData oldData;

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

        [Tooltip("Emitted when data has been changed")]
        public UnityEvent OnDataChanged;

        [Tooltip("Gaze on Button (hover with mouse).")]
        public UnityEvent OnHighlighted;

        private void Awake()
        {
            if (OnDataChanged == null)
                OnDataChanged = new UnityEvent();
        }

        public void UpdateData()
        {
            if (OnDataChanged != null && 
                (data == null || oldData == null || data.compare(oldData)))
            {
                OnDataChanged.Invoke();
                oldData = data.copy();
            }
        }

        private void OnEnable()
        {
            UpdateData();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            // we want the event to be invoked ASAP, but in the editor we need
            // to delay the call otherwise we are not allowed to 
            // instantiate, destroy or transform objects
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UpdateData();
            };
#else
            UpdateData();
#endif
        }
    }
}