using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRUi
{
    [ExecuteInEditMode]
    public class MRUiCircleButtonGroup : MonoBehaviour
    {
        // TODO: combine with MRUiButtonGroup?
        [Tooltip("amount of buttons that can be selected at once. For a select-like behaviour use 1, 0 means all options can be selected")]
        public int maxSelect = 0;

        public GameObject CircleButtonPrefab;

        public List<MRUiButtonData> data;

        [HideInInspector]
        public bool forceUpdate = false;

        // "number of segments the whole circle consists of. this defines the amount of vertices/triangles used"
        // TODO: calculate - espeically for 5 and 7
        private int parts = 36;

        [Tooltip("radius from center to circle")]
        public float innerRadius = .04f;

        [Tooltip("radius from center to outer circle")]
        public float outerRadius = .1f;

        [Tooltip("width of the circle")]
        public float width = .04f;

        // Use this for initialization
        void Start()
        {
            forceUpdate = true;
        }

        private void OnValidate()
        {
            forceUpdate = true;
        }

        public void Update()
        {
            if (forceUpdate)
            {
                updateData();
                forceUpdate = false;
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

            if (data != null && CircleButtonPrefab != null && data.Count > 0)
            {
                // create new buttons and rotate them
                float angle = 360 / data.Count;
                for (int i = 0; i < data.Count; i++)
                {
                    MRUiButtonData buttonData = data[i];
                    GameObject btn = Instantiate(CircleButtonPrefab, transform);

                    //btn.GetComponent<MRUiButton>().OnPressed.AddListener(delegate { OnButtonPressed(buttonData); });
                    btn.GetComponent<MRUiButton>().data = buttonData;
                    btn.GetComponent<MRUiButton>().updateData();
                    MRUiCircleButtonSegment segment = btn.GetComponentInChildren<MRUiCircleButtonSegment>();
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

                }
            }
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

        public void updateData()
        {
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
