using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// change text content when button data changes
/// </summary>
namespace MRUi
{
    public class MRUiButtonText : MonoBehaviour
    {
        private Text text;

        public void updateData()
        {
            MRUiButton btn = GetComponent<MRUiButton>();
            if (text == null)
            {
                text = GetComponentInChildren<Text>();
            }
            text.text = btn.data.title;
        }
    }
}
