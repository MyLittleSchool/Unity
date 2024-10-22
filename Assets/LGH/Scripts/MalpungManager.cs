using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{

    public class MalpungManager : MonoBehaviour
    {
        private Resolution resolution;
        public Transform playerMalpungTransform;
        private RectTransform rectTransform;
        //private RectTransform childRectTransform;
        void Start()
        {
            resolution = GetComponentInParent<Resolution>();
            rectTransform = GetComponent<RectTransform>();
            //childRectTransform = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        }

        void Update()
        {
            MalpungPos();
            //float childWidth = childRectTransform.sizeDelta.x;
            //float childWidthMax = Mathf.Clamp(childWidth, 0, 500);
            //childRectTransform.sizeDelta = new Vector2(childWidthMax, childRectTransform.sizeDelta.y);
            
        }

        private void MalpungPos()
        {
            Vector3 malpungPos = Camera.main.WorldToScreenPoint(playerMalpungTransform.position + (transform.up * 0.5f));
            rectTransform.anchoredPosition =  resolution.GetCoord(malpungPos);
        }
    }

}