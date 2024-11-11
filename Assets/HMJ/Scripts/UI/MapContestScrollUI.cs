using MJ;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using static MapRegisterDataUI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

public class MapContestScrollUI : ScrollUI
{
    public MapRegisterScrollUI myMapRegisterScrollUIcp;

    bool settingImage = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        LoadMapData();
    }

    public override void AddItem()
    {
        GameObject registerObject = myMapRegisterScrollUIcp.GetRegisterGameObject();
        if(registerObject)
        {
            MapRegisterData mapRegisterData = registerObject.GetComponent<MapRegisterDataUI>().GetRegisterData();

            MapContestLoader.GetInstance().SendMapContestData(CaptureManager.GetInstance().GetCapturePath(), mapRegisterData);
            SceneUIManager.GetInstance().OnMapSuccessRegisterPanel();
        }

    }

    public void LoadMapData()
    {
        StartCoroutine(WaitForConditionToBeTrue());
    }

    public void LoadMapChild()
    {
        int i = 0;
        foreach (MapContestData mapContestData in MapContestLoader.GetInstance().mapDatas.response)
        {
            GameObject item = Instantiate(prefab, content);
            //item.set
            MJ.MapContestDataUI mapContestDataUI = item.GetComponent<MJ.MapContestDataUI>();

            MapRegisterData mapRegisterData = new MapRegisterData();
            mapRegisterData.title = mapContestData.title;
            mapRegisterData.Description = mapContestData.description;
            mapRegisterData.likes = mapContestData.likeCount;
            mapRegisterData.views = mapContestData.viewCount;
            mapContestDataUI.SetRegisterData(mapRegisterData, MapContestLoader.GetInstance().sprites[i], mapContestData.furnitureList) ;

            itemlist.Add(item);
            imageList.Add(item.GetComponent<Image>());
            i++;
        }

        SceneUIManager.GetInstance().OnMapSuccessRegisterPanel();
    }


    public void ResetColor()
    {
        foreach (var image in imageList)
            image.color = Color.white;
    }

    private void OnEnable()
    {
        ResetData();
        // 맵 데이터 로드
        MapContestLoader.GetInstance().LoadMapData();

        // 스프라이트도 로드 

    }

    public void ResetData()
    {
        itemlist.Clear();
        imageList.Clear();
        content.DetachChildren();
    }

    IEnumerator WaitForConditionToBeTrue()
    {
        Debug.Log("Waiting for condition...");

        // 조건이 true가 될 때까지 대기
        yield return new WaitUntil(() => MapContestLoader.GetInstance().LoadSpriteComplete());

        LoadMapChild();
        Debug.Log("Condition met! Proceeding...");
    }
}
