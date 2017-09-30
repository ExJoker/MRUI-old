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
            updateIcon();
        }

        public void OnDisable()
        {
            RemoveEvents();
        }

        public void AddEvents()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            btnIcon.OnIconChanged.AddListener(updateIcon);

            CircleButtonSegment segment = GetComponentInChildren<CircleButtonSegment>();
            segment.OnAngleChanged.AddListener(updateIcon);
        }

        public void RemoveEvents()
        {
            ButtonIcon btnIcon = GetComponent<ButtonIcon>();
            if (btnIcon != null)
            {
                btnIcon.OnIconChanged.RemoveListener(updateIcon);
            }

            CircleButtonSegment segment = GetComponentInChildren<CircleButtonSegment>();
            if (segment != null)
            {
                segment.OnAngleChanged.RemoveListener(updateIcon);
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            updateIcon();
        }
#endif

        // Use this for initialization
        public void updateIcon()
        {
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
            Button btn = GetComponent<MRUI.Button>();
            if (btn != null && btn.data != null)
            {
                btnIcon.icon.transform.localScale = btn.data.iconScale;
            }
                
            btnIcon.icon.transform.localPosition = fixPosition;
        }
    }
}