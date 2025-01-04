using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SW {
    public class ReportPanel : MonoBehaviour
    {
        public TMP_Text targetText;
        public TMP_InputField reasonInputField;
        public Button confirmButton;
        public void SetActivePanel(bool value)
        {
            reasonInputField.text = "";
            gameObject.SetActive(value);
        }
        public void SetInfo(string _targetText)
        {
            targetText.text = _targetText;
        }
        public void ConfirmReport()
        {
            Report.instance.ConfirmReport(reasonInputField.text);
            SetActivePanel(false);
        }
    }
}