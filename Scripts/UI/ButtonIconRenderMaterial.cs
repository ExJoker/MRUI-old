using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// apply material from button data
/// </summary>
namespace MRUI
{

    [RequireComponent(typeof(MRUI.Button))]
    [RequireComponent(typeof(MRUI.ButtonIcon))]

    [ExecuteInEditMode]
    public class ButtonIconRenderMaterial : ButtonRenderMaterial
    {

        public new void AddEvents()
        {
            base.AddEvents();
            MRUI.ButtonIcon icon = GetComponent<MRUI.ButtonIcon>();
            icon.OnIconChanged.AddListener(updateData);
        }

        public new void RemoveEvents()
        {
            MRUI.ButtonIcon btn = GetComponent<MRUI.ButtonIcon>();
            if (btn != null)
            {
                btn.OnIconChanged.RemoveListener(updateData);
            }
        }

        public override void UpdateMaterial()
        {
            MRUI.ButtonIcon icon = GetComponent<MRUI.ButtonIcon>();
            MRUI.Button btn = GetComponent<Button>();
            if (btn.data.material.normal != null)
            {
                icon.SetMaterial(btn.data.material.normal);
            }
        }
    }
}