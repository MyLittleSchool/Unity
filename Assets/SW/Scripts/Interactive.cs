using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SW
{
    public abstract class Interactive : MonoBehaviour
    {
        private Material material;
        private SpriteRenderer spriteRenderer;
        public string interacterName = "»óÈ£ÀÛ¿ë´õ¹Ì";
        // Start is called before the first frame update
        protected virtual void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            material = new Material(spriteRenderer.material);
            spriteRenderer.material = material;
        }
        public abstract void Interact();
        public string GetInfo()
        {
            return interacterName;
        }
        public virtual void HighlightOff()
        {
            material.SetInt("_On", 0);
            print("²¨Áü");
        }
        public void HighlightOn()
        {
            material.SetInt("_On", 1);
            print("ÄÑÁü");
        }
    }
}