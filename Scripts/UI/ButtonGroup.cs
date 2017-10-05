using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRUI {

    [System.Serializable]
    public class OnSelectedEvent : UnityEvent<string> { }

    public class ButtonGroup : MonoBehaviour {

        [Tooltip("amount of buttons that can be selected at once. For a select-like behaviour use 1, 0 means all options can be selected")]
        public int maxSelect = 0;

        public float buttonDistanceVertical = 0.11f;
        public float buttonDistanceHorizontal = 0.22f;

        private int OnPressedHashCode = 0;

        public int maxRows = 3;

        public List<ButtonData> data;
        // amount of items in last validate-call 
        // used to efficiantly only create new button-instances when needed

        /// <summary>
        /// events that gets thrown when user presses one of the buttons
        /// </summary>
        [SerializeField]
        public OnSelectedEvent OnSelected;

        public UnityEvent OnDataChange;

        public GameObject ButtonPrefab;

        private void Awake()
        {
            if (OnDataChange == null)
                OnDataChange = new UnityEvent();
        }

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

        void DestroyButtons()
        {
            if (this == null || transform == null || transform.childCount == 0)
            {
                return;
            }
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
            if (ButtonPrefab != null)
            {
                for (int i = transform.childCount; i < data.Count; i++)
                {
                    ButtonData buttonData = data[i];
                    Instantiate(ButtonPrefab, transform);

                }
            }
            else
            {
                Debug.LogError("No Prefab set for button Group");
            }
        }

        void UpdateButtons()
        {
            if (ButtonPrefab == null)
            {
                Debug.LogError("No Prefab set for button Group");
                return;
            }
            // center buttons
            int rows = System.Math.Min(maxRows, data.Count);
            int columns = (data.Count - 1) / maxRows + 1;
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
                btn.UpdateData();

                int j = i % rows;
                btnInst.transform.localPosition = new Vector3(
                    ((i / rows) - ((columns - 1) / 2f)) * buttonDistanceHorizontal,
                    -(j - (rows - 1) / 2f) * buttonDistanceVertical);
                if (OnDataChange != null)
                {
                    OnDataChange.Invoke();
                }
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

        public void OnDestroy()
        {
            this.data.Clear();
            DestroyButtons();
            if (OnSelected != null)
            {
                OnSelected.RemoveAllListeners();
            }
        }

        public void UpdateData() {
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
    }
} 