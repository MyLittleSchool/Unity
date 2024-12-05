using GH;
using SW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameInteractButton : MonoBehaviour
{
    private static gameInteractButton instance;

    private Button gameInteractBut;
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
        Debug.Log("버튼 체크: " + bButton);
    }

    public void InitButton()
    {
        gameInteractBut = GetComponent<Button>();
        bButton = false;
    }

    IEnumerator interactButtonDown()
    {
        bButton = true;
        yield return null;
        bButton = false;
    }

    public void ButtonDown()
    {
        StartCoroutine(interactButtonDown());
    }

    public bool GetButtonDown()
    {
        return bButton;
    }

}
