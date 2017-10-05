using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MRUI
{
    [ExecuteInEditMode]
    public class DialogTitle : MonoBehaviour
    {
        public GameObject background;
        public Text text;
        public RectTransform rectTransform;

        [Tooltip("height of the text canvas")]
        public float textHeight = 120f;

        [Tooltip("width of the text canvas")]
        public float columnWidth = 420f;

        [Tooltip("Border between text Canvas and Text ui component")]
        public float border = 8f;

        private int columns = 0;
        private float scaleX = 0;

        public void OnEnable()
        {
            text = text ?? GetComponentInChildren<Text>();
            rectTransform = rectTransform ?? GetComponentInChildren<RectTransform>();
            if (background == null)
            {
                foreach (Transform child in transform)
                {
                    if (child.gameObject.name == "Background")
                    {
                        background = child.gameObject;
                        break;
                    }
                }
            }
        }

        public void OnValidate()
        {
            if (columns > 0 && scaleX > 0)
            {
                SetColumns(this.columns, this.scaleX);
            }
        }

        public void SetColumns(int columns, float scaleX)
        {
            this.columns = columns;
            this.scaleX = scaleX;
            Vector3 scale = background.transform.localScale;
            scale.x = columns * scaleX;
            background.transform.localScale = scale;

            text.rectTransform.sizeDelta = new Vector2(columnWidth * columns - border, textHeight - border);
            rectTransform.sizeDelta = new Vector2(columnWidth * columns, textHeight);
        }
    }
}