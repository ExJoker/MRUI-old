using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// move text according to rotation
/// </summary>

namespace MRUI
{
    [RequireComponent(typeof(MRUI.Button))]
    [RequireComponent(typeof(MRUI.ButtonText))]

    [ExecuteInEditMode]
    public class CircleButtonTextPosition : MonoBehaviour
    {
        public GameObject textGo;

        private CircleButtonSegment segment;

        public void OnEnable()
        {
            AddEvents();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UpdateData();
            };
#else
            UpdateData();
#endif
        }

        public void OnDisable()
        {
            RemoveEvents();
        }

        public void AddEvents()
        {
            Button btn = GetComponent<Button>();
            btn.OnDataChanged.AddListener(UpdateData);
        }

        public void RemoveEvents()
        {
            Button btn = GetComponent<Button>();
            if (btn != null)
            {
                btn.OnDataChanged.RemoveListener(UpdateData);
            }
        }

        public void UpdateData()
        {
            if (this == null)
            {
                return;
            }
            UpdatePosition();
        }

        void UpdatePosition()
        {
            segment = GetComponentInChildren<CircleButtonSegment>();
            if (segment == null)
            {
                return;
            }
            // center on curve
            float angle = segment.angle;
            angle += 180f / segment.segments;
            if (angle < 0f)
            {
                angle += 360;
            }
            else if (angle >= 360f)
            {
                angle = angle % 360;
            }
            Text text = textGo.GetComponentInChildren<Text>();
            float cx = angle / 180f * Mathf.PI;
            float cy = cx;
            cx = segment.outerRadius * Mathf.Cos(cx);
            cy = segment.outerRadius * Mathf.Sin(cy);

            if (angle >= 90 - 15 && angle <= 90 + 15)
            {
                cy = .12f;
            }
            if (angle >= 270 - 15 && angle <= 270 + 15)
            {
                cy = -.12f;
            }

            if (angle < 90 || angle > 270)
            {
                cx += .09f;
                text.alignment = TextAnchor.MiddleLeft;
            }
            else
            {
                cx += -.09f;
                text.alignment = TextAnchor.MiddleRight;
            }

            if (angle >= 90 - 5 && angle <= 90 + 5)
            {
                cx = 0f;
                text.alignment = TextAnchor.MiddleCenter;
            }
            if (angle >= 270 - 5 && angle <= 270 + 5)
            {
                cx = 0f;
                text.alignment = TextAnchor.MiddleCenter;
            }
            textGo.transform.localPosition = new Vector3(cx, cy, 0);
        }
        
    }
}