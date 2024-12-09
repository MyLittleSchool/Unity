using GH;
using SW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameInteractButton : MonoBehaviour
{
    private static gameInteractButton instance;

    private bool bButton;

    public static gameInteractButton GetInstance()
    {
        return instance;
    }



    private void Awake()
    {
        instance = this;
        InitButton();
    }

    private void Update()
    {
    }

    public void InitButton()
    {
        bButton = false;
    }

    IEnumerator interactButtonDown()
    {
        Debug.Log("Button Pressed");
        bButton = true;
        yield return null;
        bButton = false;
    }

    public void ButtonDown()
    {
        if(!bButton)
            StartCoroutine(interactButtonDown());
    }

    public bool GetButtonDown()
    {
        return bButton;
    }

}
