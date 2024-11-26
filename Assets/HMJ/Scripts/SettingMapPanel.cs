using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMapPanel : MonoBehaviour
{
    private static SettingMapPanel instance;
    public static SettingMapPanel GetInstance()
    {
        return instance;
    }

    MapRegisterScrollUI mapRegisterScrollUICom;
    MapContestScrollUI mapContestScrollUICom;

    private void Awake()
    {
        if (null == instance)
            instance = this;

        InitMapPanelUI();
    }

    public void InitMapPanelUI()
    {
        mapRegisterScrollUICom = GetComponentInChildren<MapRegisterScrollUI>();
        mapContestScrollUICom = GetComponentInChildren<MapContestScrollUI>();

        mapRegisterScrollUICom.InitScrollUI();
        mapContestScrollUICom.InitScrollUI();
    }

    public void OnMapContestPanel()
    {
        mapContestScrollUICom.MapDataLoad();
    }

}
