using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// apply material from button data
/// </summary>
namespace MRUI
{

    [RequireComponent(typeof(MRUI.Button))]
    [ExecuteInEditMode]
    public class ButtonRenderMaterial : MonoBehaviour
    {
        // renderer is needed to change the material when highlighted (hovered) or pressed (tabbed)
        [Tooltip("Rednerer that we will assign the material to. If this is null we will try to get the Renderer from the component.")]
        public Renderer rend;

#if UNITY_EDITOR
        private bool forceUpdate = false;
#endif

        public void Start()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            rend = (rend == null) ? GetComponentInChildren<Renderer>() : rend;
            btn.OnDataChanged.AddListener(updateData);
        }

        public void OnDestroy()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            if (btn != null)
            {
                btn.OnDataChanged.RemoveListener(updateData);
            }
        }

        public void updateData()
        {
            // update text next frame - we setting the text directly does not work in Editor!
#if UNITY_EDITOR
            forceUpdate = true;
#else
            UpdateMaterial();
#endif
        }

        private void UpdateMaterial()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
#if UNITY_EDITOR
            forceUpdate = false;
#endif
            if (btn.data.material.normal != null)
            {
                rend.material = btn.data.material.normal;
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (forceUpdate)
            {
                UpdateMaterial();
            }
        }
#endif
    }
}