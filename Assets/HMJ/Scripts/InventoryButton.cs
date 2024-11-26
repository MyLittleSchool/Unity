using GH;
using MJ;
using Ookii.Dialogs;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public Button inventoryButton;

    private void Awake()
    {
        InitButtons();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitButtons()
    {
        inventoryButton.onClick.AddListener(() => InventorySystem.GetInstance().SetChoiceItem(GetComponentInParent<Item>()));
    }
}
