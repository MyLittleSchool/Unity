using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SW
{
    public class PlayerInteracter : MonoBehaviour
    {
        private List<Interactive> inTrigged;
        public GameObject ui;
        public TMP_Text uiText;
        public RectTransform panel;
        void Start()
        {
            inTrigged = new List<Interactive>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            inTrigged.Add(collision.GetComponent<Interactive>());
            if (inTrigged.Count == 1)
            {
                uiText.text = inTrigged.First().GetInfo();
                inTrigged.First().HighlightOn();
                ui.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(panel);

            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Interactive i = collision.GetComponent<Interactive>();
            i.HighlightOff();
            inTrigged.Remove(i);
            
            if (inTrigged.Count != 0)
            {
                uiText.text = inTrigged.First().GetInfo();
                inTrigged.First().HighlightOn();
                ui.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(panel);
            }
            else
            {
                uiText.text = "";
                ui.SetActive(false);

            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && inTrigged.Count != 0)
            {
                inTrigged.First().Interact();
            }
        }
    }
}