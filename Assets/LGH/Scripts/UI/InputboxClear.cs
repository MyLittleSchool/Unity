using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace GH
{

    public class InputboxClear : MonoBehaviour
    {
        private Button xButton;
        private TMP_InputField inputFields;

        void Start()
        {
            xButton = GetComponent<Button>();
            inputFields = GetComponentInParent<TMP_InputField>();
            xButton.onClick.AddListener(TextClear);
        }


        private void TextClear()
        {
            inputFields.text = "";
        }
    }

}