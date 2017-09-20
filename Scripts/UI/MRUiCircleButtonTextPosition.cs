using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MRUi
{
    [ExecuteInEditMode]
    public class MRUiCircleButtonTextPosition : MonoBehaviour
    {
        private Text text;
        public GameObject textGo;
        

        private MRUiCircleButtonSegment segment;

        void Start()
        {
            segment = GetComponent<MRUiCircleButtonSegment>();
            if (segment == null)
            {
                return;
            }
            text = textGo.GetComponentInChildren<Text>();
            updateTextPos();
            // center on curve
            float angle = segment.angle;
            angle += 180f / segment.segments;
            if (angle > 360f)
            {
                angle -= 360;
            }

            float cx = angle / 180f * Mathf.PI;
            float cy = cx;
            cx = segment.outerRadius * Mathf.Cos(cx);
            cy = segment.outerRadius * Mathf.Sin(cy);
            textGo.transform.localPosition = new Vector3(cx, cy, 0);
        }

        public void updateTextPos()
        {
            //text.alignment = TextAnchor.MiddleLeft;
        }
    }
}