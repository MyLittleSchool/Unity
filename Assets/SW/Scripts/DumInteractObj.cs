using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumInteractObj : Interact, IInteract
{
    public string interacterName = "»óÈ£ÀÛ¿ë´õ¹Ì";
    public string GetInfo()
    {
        return interacterName;
    }


    public void Interact()
    {
        print(interacterName);
    }
    public void HighlightOff()
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
