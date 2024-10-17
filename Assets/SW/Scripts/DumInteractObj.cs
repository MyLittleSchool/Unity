using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumInteractObj : MonoBehaviour, IInteract
{
    public string interacterName = "상호작용더미";
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
        
    }

    public void HighlightOn()
    {
        
    }
}
