using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SW
{
    public class PlayerInteracter : MonoBehaviour
    {
        private List<IInteract> inTrigged;
        public GameObject ui;
        public TMP_Text uiText;
        void Start()
        {
            inTrigged = new List<IInteract>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            inTrigged.Add(collision.GetComponent<IInteract>());
            ui.SetActive(true);
            if (inTrigged.Count == 1)
            {
                uiText.text = inTrigged.First().GetInfo();
                inTrigged.First().HighlightOn();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            IInteract i = collision.GetComponent<IInteract>();
            i.HighlightOff();
            inTrigged.Remove(i);
            
            if (inTrigged.Count != 0)
            {
                uiText.text = inTrigged.First().GetInfo();
                inTrigged.First().HighlightOn();
            }
            else
            {
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