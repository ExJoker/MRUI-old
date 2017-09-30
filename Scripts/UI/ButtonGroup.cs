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

        public int maxRows = 3;

        public List<ButtonData> data;

        /// <summary>
        /// events that gets thrown when user presses one of the buttons
        /// </summary>
        [SerializeField]
        public OnSelectedEvent OnSelected;

        public GameObject ButtonPrefab;
        
	    void OnEnable() {
            UpdateData();
        }

        private void OnValidate()
        {
            UpdateData();
        }

        void DestroyButtons()
        {
            List<GameObject> children = new List<GameObject>();
            // we can not delete items from a list while iterating over it
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }
            for (int i = children.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(children[i]);
            }
        }

        void updateButtons()
        {
            if (this == null)
            {
                // because this is a delayed call it might be that the object has already been destroyed
                return;
            }

            // delay creation/deletion of objects
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this == null)
                {
                    return;
                }
                DestroyButtons();
                CreateButtons();
            };
        }

        void CreateButtons()
        {
            if (data != null && ButtonPrefab != null && data.Count > 0)
            {
                // create new buttons and center them
                int rows = System.Math.Min(maxRows, data.Count);
                int columns = data.Count / maxRows + 1;
                for (int i = 0; i < data.Count; i++)
                {
                    ButtonData buttonData = data[i];
                    GameObject btnInst = Instantiate(ButtonPrefab, transform);

                    MRUI.Button btn = btnInst.GetComponent<MRUI.Button>();
                    btn.OnPressed.AddListener(delegate { OnButtonPressed(buttonData); });
                    btn.data = buttonData;
                    btn.UpdateData();

                    int j = i % rows;
                    btnInst.transform.localPosition = new Vector3(
                        ((i / rows) - ((columns - 1) / 2f)) * buttonDistanceHorizontal,
                        -(j - (rows - 1) / 2f) * buttonDistanceVertical);
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

        public void UpdateData() {
            updateButtons();
        }
    }
}