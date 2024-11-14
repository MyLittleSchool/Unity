using MJ;
using SW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher2 : Interactive
{
    public override void Interact()
    {
        SceneUIManager.GetInstance().OnMapConfirmPanel();
    }
}
