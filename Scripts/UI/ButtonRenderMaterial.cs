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

        public void Start()
        {
            rend = (rend == null) ? GetComponentInChildren<Renderer>() : rend;
        }

        public void OnEnable()
        {
            AddEvents();
        }


        public void OnDisable()
        {
            RemoveEvents();
        }

        public void AddEvents()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            btn.OnDataChanged.AddListener(updateData);
        }

        public void RemoveEvents()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            if (btn != null)
            {
                btn.OnDataChanged.RemoveListener(updateData);
            }
        }

        public void updateData()
        {
            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            MRUI.Button btn = GetComponent<MRUI.Button>();
            if (btn.data.material.normal != null)
            {
                rend.material = btn.data.material.normal;
            }
        }
    }
}