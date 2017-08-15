using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloUi {
    public class HoloUiButtonGroup : MonoBehaviour {
        [Tooltip("amount of buttons that can be selected at once. For a select-like behaviour use 1, 0 means all options can be selected")]
        public int maxSelect = 0;

        public float buttonDistanceVertical = 3f;
        public float buttonDistanceHorizontal = 7f;

        public int maxRows = 3;

        public List<HoloUiButtonData> data;

        public GameObject HoloButtonPrefab;
        
	    // Use this for initialization
	    void Start() {
            updateData();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            updateDataDelayed();
#else
            updateData();
#endif
        }
        
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

#if UNITY_EDITOR
        void updateDataDelayed()
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                updateData();
            };
        }
#endif

        // Update is called once per frame
        void updateData() {
            
            if (this == null)
            {
                // because this is a delayed call it might be that the class already has been destroyed
                return;
            }
            destroyButtons();
            if (data != null)
            {
                // create new buttons and center them
                int rows = System.Math.Min(maxRows, data.Count);
                int columns = data.Count / maxRows + 1;
                for (int i = 0; i < data.Count; i++)
                {
                    HoloUiButtonData buttonData = data[i];
                    GameObject btn = Instantiate(HoloButtonPrefab, transform);
                    btn.GetComponent<HoloUiButton>().data = buttonData;
                    btn.GetComponent<HoloUiButton>().updateData();

                    int j = i % rows;
                    btn.transform.localPosition = new Vector3(
                        ((i / rows) - ((columns - 1) / 2f)) * buttonDistanceHorizontal,
                        -(j - (rows - 1) / 2f) * buttonDistanceVertical);
                }
            }
            
        }
    }
}