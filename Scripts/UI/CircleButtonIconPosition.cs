using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRUI
{
    [RequireComponent(typeof(MRUI.Button))]
    [RequireComponent(typeof(ButtonIcon))]

    [ExecuteInEditMode]
    public class CircleButtonIconPosition : MonoBehaviour
    {

        private Vector3 fixPosition = new Vector3(0, 0, -0.021f);

        public void OnEnable()
        {
            AddEvents();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                UpdateIcon();
            };
#else
            UpdateIcon();
#endif
        }

        public void OnDisable()
        {
            RemoveEvents();
        }

        public void AddEvents()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            btnIcon.OnIconChanged.AddListener(UpdateIcon);

            Button btn = GetComponent<Button>();
            btn.OnDataChanged.AddListener(UpdateIcon);
        }

        public void RemoveEvents()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            if (btnIcon != null)
            {
                btnIcon.OnIconChanged.RemoveListener(UpdateIcon);
            }

            Button btn = GetComponent<Button>();
            if (btn != null)
            {
                btn.OnDataChanged.RemoveListener(UpdateIcon);
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            UpdateIcon();
        }
#endif

        // Use this for initialization
        public void UpdateIcon()
        {
            if (this == null)
            {
                return;
            }
            CircleButtonSegment segment = GetComponentInChildren<CircleButtonSegment>();
            if (segment != null)
            {
                float radius = segment.innerRadius + (segment.outerRadius - segment.innerRadius) / 2;
                float angle = segment.angle;
                float cx = (angle + (360f / segment.segments / 2)) / 180f * Mathf.PI;
                float cy = cx;
                fixPosition.x = radius * Mathf.Cos(cx);
                fixPosition.y = radius * Mathf.Sin(cy);
                fixPosition.z = -(segment.width / 2 + .001f);
            }
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            if (btnIcon == null || btnIcon.icon == null)
            {
                return;
            }
            btnIcon.icon.transform.localPosition = fixPosition;
        }
    }
}