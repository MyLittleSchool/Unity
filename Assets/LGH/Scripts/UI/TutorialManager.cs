using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TutorialManager;

public class TutorialManager : MonoBehaviour
{

    public enum TutoPage
    {
        Start,
        One,
        Two,
        Three,
        end
    }

    public TutoPage tutoPage;


    public guideInfo guideInfo;
    public List<Sprite> guideImages;

    public GameObject guidePanel;

    public bool lawStart = false;
    void Start()
    {
        tutoPage = TutoPage.Start;
        guideInfo.beforeButton.gameObject.SetActive(false);
        guideInfo.startButton.gameObject.SetActive(false);

        guideInfo.beforeButton.onClick.AddListener(BeforePage);
        guideInfo.nextButton.onClick.AddListener(NextPage);
        guideInfo.startButton.onClick.AddListener(StartButton);

    }

    void Update()
    {

    }
    public void BeforePage()
    {
        tutoPage--;
        guideInfo.imageWindow.sprite = guideImages[(int)tutoPage];


        if (tutoPage == TutoPage.Start)
        {
            guideInfo.beforeButton.gameObject.SetActive(false);
            guideInfo.nextButton.gameObject.SetActive(true);


        }
        else if (tutoPage == TutoPage.end)
        {
            guideInfo.nextButton.gameObject.SetActive(false);
            guideInfo.beforeButton.gameObject.SetActive(true);
            guideInfo.startButton.gameObject.SetActive(true);


        }
        else
        {
            guideInfo.nextButton.gameObject.SetActive(true);
            guideInfo.beforeButton.gameObject.SetActive(true);
            guideInfo.startButton.gameObject.SetActive(false);

        }
    }

    public void NextPage()
    {
        tutoPage++;
        guideInfo.imageWindow.sprite = guideImages[(int)tutoPage];
        guideInfo.nextButton.gameObject.SetActive(true);


        if (tutoPage == TutoPage.Start)
        {
            guideInfo.beforeButton.gameObject.SetActive(false);
            guideInfo.nextButton.gameObject.SetActive(true);

        }
        else if (tutoPage == TutoPage.end)
        {
            guideInfo.nextButton.gameObject.SetActive(false);
            guideInfo.beforeButton.gameObject.SetActive(true);
            guideInfo.startButton.gameObject.SetActive(true);
        }
        else
        {
            guideInfo.nextButton.gameObject.SetActive(true);
            guideInfo.beforeButton.gameObject.SetActive(true);
            guideInfo.startButton.gameObject.SetActive(false);
        }
    }

    public void StartButton()
    {
        lawStart = true;
        guidePanel.SetActive(false);
    }
}
