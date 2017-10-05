using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MRUI
{
    [ExecuteInEditMode]
    public class Dialog : MonoBehaviour
    {
        public string title;
        public DialogTitle dialogTitle;
        public ButtonGroup buttonGroup;

        public void OnEnable()
        {
            dialogTitle = dialogTitle ?? GetComponentInChildren<DialogTitle>();
            buttonGroup = buttonGroup ?? GetComponentInChildren<ButtonGroup>();
            if (buttonGroup != null)
            {
                buttonGroup.OnDataChange.AddListener(OnDataChange);
            }
        }

        public void OnDataChange()
        {
            if (buttonGroup != null)
            {
                int columns = (buttonGroup.data.Count) / buttonGroup.maxRows;
                if (buttonGroup.data.Count % buttonGroup.maxRows > 0)
                {
                    columns++;
                }
                float scaleX = buttonGroup.transform.localScale.x * buttonGroup.buttonDistanceHorizontal;
                dialogTitle.SetColumns(columns, scaleX);
            }
        }

        public void OnValidate()
        {
            if (dialogTitle != null && dialogTitle.text != null)
            {
                dialogTitle.text.text = title;
            }
        }

        public void OnDisable()
        {
            buttonGroup.OnDataChange.RemoveListener(OnDataChange);
        }
    }
}