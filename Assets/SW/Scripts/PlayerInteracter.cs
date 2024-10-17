using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SW
{
    public class PlayerInteracter : MonoBehaviour
    {
        private List<Collider2D> inTrigged;
        public GameObject ui;
        public TMP_Text uiText;
        void Start()
        {
            inTrigged = new List<Collider2D>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            inTrigged.Add(collision);
            ui.SetActive(true);
            uiText.text = inTrigged.First().GetComponent<IInteract>().GetInfo();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            inTrigged.Remove(collision);
            if (inTrigged.Count != 0)
            {
                uiText.text = inTrigged.First().GetComponent<IInteract>().GetInfo();
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
                inTrigged.First().GetComponent<IInteract>().Interact();
            }
        }
    }
}