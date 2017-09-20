using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRUi {

    public class MRUiButtonGroup : MonoBehaviour {


        [Tooltip("amount of buttons that can be selected at once. For a select-like behaviour use 1, 0 means all options can be selected")]
        public int maxSelect = 0;

        public float buttonDistanceVertical = 3f;
        public float buttonDistanceHorizontal = 7f;

        public int maxRows = 3;

        public List<MRUiButtonData> data;

        [HideInInspector]
        public bool forceUpdate = false;

        public GameObject ButtonPrefab;
        
	    void Start() {
            updateData();
        }

        private void OnValidate()
        {
            updateData();
        }

        public delegate void OnSelectedDelegate(MRUiButtonData data);
        /// <summary>
        /// events that gets thrown when user presses one of the buttons
        /// </summary>
        public event OnSelectedDelegate OnSelected;

        void destroyButtons()
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
            destroyButtons();
            if (data != null && ButtonPrefab != null && data.Count > 0)
            {
                // create new buttons and center them
                int rows = System.Math.Min(maxRows, data.Count);
                int columns = data.Count / maxRows + 1;
                for (int i = 0; i < data.Count; i++)
                {
                    MRUiButtonData buttonData = data[i];
                    GameObject btn = Instantiate(ButtonPrefab, transform);
                    
                    btn.GetComponent<MRUiButton>().OnPressed.AddListener(delegate { OnButtonPressed(buttonData); });
                    btn.GetComponent<MRUiButton>().data = buttonData;
                    btn.GetComponent<MRUiButton>().updateData();

                    int j = i % rows;
                    btn.transform.localPosition = new Vector3(
                        ((i / rows) - ((columns - 1) / 2f)) * buttonDistanceHorizontal,
                        -(j - (rows - 1) / 2f) * buttonDistanceVertical);
                }
            }
        }

        /// <summary>
        /// user selected one of the options by pressing a button 
        /// </summary>
        public void OnButtonPressed(MRUiButtonData data)
        {
            if (OnSelected != null)
            {
                OnSelected.Invoke(data);
            }
        }

        public void Update()
        {
            if (forceUpdate)
            {
                updateData();
                forceUpdate = false;
            }
        }

        public void updateData() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                updateButtons();
            };
#else
            updateButtons();
#endif
        }
    }
}