using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRUi
{
    [ExecuteInEditMode]
    public class MRUiCircleButtonIconPosition : MonoBehaviour
    {

        [Tooltip("Scale for the icon")]
        public Vector3 fixScale = new Vector3(0.02f, 0.02f, 1f);

        private Vector3 fixPosition = new Vector3(0, 0, -0.021f);

        public void Start()
        {
            updateIcon();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            updateIcon();
        }
#endif

        private void _updateIcon()
        {

        }

        // Use this for initialization
        public void updateIcon()
        {
            MRUiCircleButtonSegment segment = GetComponentInChildren<MRUiCircleButtonSegment>();
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
            GameObject icon = GetComponent<MRUiButtonIcon>().icon;
            if (icon != null)
            {
                icon.transform.localScale = fixScale;
                icon.transform.localPosition = fixPosition;
                Debug.Log(fixScale.x);
                Debug.Log(fixScale.y);
            }
        }
    }
}