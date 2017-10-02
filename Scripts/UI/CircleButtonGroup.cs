using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRUI
{
    [ExecuteInEditMode]
    public class CircleButtonGroup : MonoBehaviour
    {
        // TODO: combine with MRUiButtonGroup?
        [Tooltip("amount of buttons that can be selected at once. For a select-like behaviour use 1, 0 means all options can be selected")]
        public int maxSelect = 0;

        public GameObject CircleButtonPrefab;

        public List<ButtonData> data;

        // "number of segments the whole circle consists of. this defines the amount of vertices/triangles used"
        // TODO: calculate - espeically for 5 and 7
        private int parts = 36;

        [Tooltip("radius from center to circle")]
        public float innerRadius = .04f;

        [Tooltip("radius from center to outer circle")]
        public float outerRadius = .1f;

        [Tooltip("width of the circle")]
        public float width = .04f;

        [SerializeField]
        public OnSelectedEvent OnSelected;

        private int OnPressedHashCode = 0;

        void OnEnable()
        {
            UpdateData();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UpdateData();
            };
#else
            UpdateData();
#endif
        }


        void DestroyButtons()
        {
            // we can not delete items from a list while iterating over it
            for (int i = transform.childCount - 1; i >= data.Count; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        void CreateButtons()
        {
            if (this == null || transform == null)
            {
                return;
            }
            if (CircleButtonPrefab == null)
            {
                Debug.LogError("No Prefab set for button Group");
            }
            else
            {
                for (int i = transform.childCount; i < data.Count; i++)
                {
                    ButtonData buttonData = data[i];
                    Instantiate(CircleButtonPrefab, transform);
                }
            }
        }

        void UpdateButtons()
        {
            if (data.Count == 0)
            {
                return;
            }
            switch (data.Count)
            {
                case 10:
                case 8:
                case 5:
                    parts = 40;
                    break;
                case 7:
                    parts = 49;
                    break;
                default:
                    parts = 36;
                    break;
            }


            // create new buttons and rotate them
            float angle = 360 / data.Count;
            for (int i = 0; i < data.Count; i++)
            {
                ButtonData buttonData = data[i];
                GameObject btnInst = transform.GetChild(i).gameObject;
                MRUI.Button btn = btnInst.GetComponent<MRUI.Button>();

                if (OnPressedHashCode != btn.OnPressed.GetHashCode())
                {
                    btn.OnPressed.AddListener(delegate { OnButtonPressed(buttonData); });
                    OnPressedHashCode = btn.OnPressed.GetHashCode();
                }
                btn.data = buttonData;
                    
                CircleButtonSegment segment = btnInst.GetComponentInChildren<CircleButtonSegment>();
                segment.parts = parts;
                segment.innerRadius = innerRadius;
                segment.outerRadius = outerRadius;
                segment.width = width;
                segment.segments = data.Count;
                int add = 0;
                switch (data.Count)
                {
                    case 5:
                        add = -16;
                        break;
                    case 4:
                        add = -45;
                        break;
                    case 3:
                        add = -30;
                        break;
                }
                segment.setAngle(angle*i+add);
                segment.UpdateData();

                // force redraw
                btn.OnDataChanged.Invoke();
            }
        }

        /// <summary>
        /// user selected one of the options by pressing a button 
        /// </summary>
        public void OnButtonPressed(ButtonData data)
        {
            if (OnSelected != null)
            {
                OnSelected.Invoke(data.data);
            }
        }

        public bool UpdateDataNextFrame = false;

        // workaround to update data from external thread (e.g. socket communication)
        public void Update()
        {
            if (UpdateDataNextFrame == true)
            {
                UpdateData();
                UpdateDataNextFrame = false;
            }
        }

        public void UpdateData()
        {
            if (this == null || transform == null)
            {
                return;
            }
            DestroyButtons();
            if (data == null)
            {
                return;
            }
            CreateButtons();
            UpdateButtons();
        }


        public void OnDestroy()
        {
            this.data.Clear();
            DestroyButtons();
            if (OnSelected != null)
            {
                OnSelected.RemoveAllListeners();
            }
        }
    }
}
