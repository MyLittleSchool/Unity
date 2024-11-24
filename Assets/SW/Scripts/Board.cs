using GH;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using SW;
using System.Linq;

public class Board : MonoBehaviour
{
    [Header("게시글 리스트")]
    public GameObject content;
    public GameObject contentPrefab;
    [Header("작성 패널")]
    public GameObject createPanel;
    public Button saveBoardButton;
    public TMP_InputField titleInputField;
    public TMP_InputField contentInputField;
    [Header("내용 패널")]
    public GameObject contentPanel;
    public GameObject boardContents;
    public TMP_Text titleText;
    public TMP_Text contentText;
    public TMP_Text likeCountText;
    public Button likeButton;
    public TMP_Text comentCountText;
    public TMP_InputField comentInputField;
    public Button saveComentButton;
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    public void SetCreatePanel()
    {
        if (createPanel.activeSelf)
        {
            createPanel.SetActive(false);
            titleInputField.text = "";
            contentInputField.text = "";
            saveBoardButton.interactable = false;
        }
        else
        {
            createPanel.SetActive(true);
        }
    }
    public void CreatePanelContentsChanged()
    {
        print(titleInputField.text.Length + "/" + contentInputField.text.Length);
        if (titleInputField.text.Length == 0 || contentInputField.text.Length == 0)
            saveBoardButton.interactable = false;
        else
            saveBoardButton.interactable = true;
    }
    public void SaveBoard()
    {
        // 생성 요청 통신
        BoardPostInfo boardPostInfo = new BoardPostInfo();
        boardPostInfo.title = titleInputField.text;
        boardPostInfo.content = contentInputField.text;
        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/board";
        info.body = JsonUtility.ToJson(boardPostInfo);
        info.contentType = "application/json";
        info.onComplete = (DownloadHandler res) =>
        {
            ToastMessage.OnMessage("고민이 등록되었습니다");
            LoadBoardData();
        };
        StartCoroutine(HttpManager.GetInstance().Post(info));
        SetCreatePanel();
        titleInputField.text = "";
        contentInputField.text = "";
    }
    [Serializable]
    private struct BoardPostInfo
    {
        public string title;
        public string content;
    }

    public void SetContentPanel()
    {
        if (contentPanel.activeSelf)
        {
            contentPanel.SetActive(false);
            comentInputField.text = "";
            saveComentButton.interactable = false;
            LoadBoardData();
        }
        else
        {
            contentPanel.SetActive(true);
        }
    }
    public void ComentInputFieldChanged()
    {
        if (comentInputField.text.Length == 0)
            saveComentButton.interactable = false;
        else
            saveComentButton.interactable = true;
    }
    public void SaveComent()
    {
        // 댓글 저장 요청 통신
        CommentInfo commentInfo = new CommentInfo();
        commentInfo.content = comentInputField.text;
        commentInfo.boardId = loadedContent.id;
        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/comment";
        info.body = JsonUtility.ToJson(commentInfo);
        info.contentType = "application/json";
        info.onComplete = (DownloadHandler res) =>
        {
            ToastMessage.OnMessage("댓글이 등록되었습니다");
            LoadComentsData(loadedContent.id);
        };
        comentInputField.text = "";
        saveComentButton.interactable = false;
        StartCoroutine(HttpManager.GetInstance().Post(info));
    }
    [Serializable]
    private struct CommentInfo
    {
        public string content;
        public int boardId;
    }

    public GameObject[] sortTabs;
    private bool sortByLike;
    public void SortByLike(bool value)
    {
        sortByLike = value;
        if (value)
        {   // 인기순
            sortTabs[0].gameObject.SetActive(false);
            sortTabs[1].gameObject.SetActive(true);
        }
        else
        {   // 최신순
            sortTabs[0].gameObject.SetActive(true);
            sortTabs[1].gameObject.SetActive(false);
        }
        LoadBoardData();
    }
    private BoardContent loadedContent;
    public void LoadBoardData()
    {
        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/board/list";
        info.onComplete = (DownloadHandler res) =>
        {
            // 제거
            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            // 생성
            BoardGetList boardGetList = JsonUtility.FromJson<BoardGetList>("{\"data\":" + res.text + "}");
            if (sortByLike)
            {
                boardGetList.data.Sort((a, b) =>
                {
                    int result = a.likeCount.CompareTo(b.likeCount);
                    if (result == 0)
                        result = a.boardId.CompareTo(b.boardId);
                    return result;
                });
            }
            for (int i = boardGetList.data.Count - 1; i >= 0; i--)
            {
                GameObject newPanel = Instantiate(contentPrefab, content.transform);
                BoardContent comp = newPanel.GetComponent<BoardContent>();
                comp.id = boardGetList.data[i].boardId;
                comp.title = boardGetList.data[i].title;
                comp.content = boardGetList.data[i].content;
                comp.like= boardGetList.data[i].likeCount;
                comp.text.text = comp.title;
                comp.likeCountText.text = comp.like.ToString();
                // 댓글 개수 구현 필요\
                //comp.comentCountText.text = comp.
                comp.button.onClick.AddListener(() =>
                {
                    loadedContent = comp;
                    contentPanel.SetActive(true);
                    titleText.text = comp.title;
                    contentText.text = comp.content;
                    likeCountText.text = comp.like.ToString();
                    comentCountText.text = comp.comentCount.ToString();
                    LoadComentsData(comp.id);
                    likeButton.onClick.AddListener(() =>
                    {
                        // 좋아요 버튼

                    });
                });
            }
        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }
    public GameObject commentPrefab;
    public void LoadComentsData(int id)
    {
        // 제거
        for (int i = 2; i < boardContents.transform.childCount; i++)
        {
            Destroy(boardContents.transform.GetChild(i).gameObject);
        }
        // 생성
        HttpManager.HttpInfo info = new HttpManager.HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/comment/" + id;
        info.onComplete = (DownloadHandler res) =>
        {
            CommentList data = JsonUtility.FromJson<CommentList>("{\"data\":" + res.text + "}");
            for (int i = 0; i < data.data.Length; i++)
            {
                // 댓글 생성
                GameObject newPanel = Instantiate(commentPrefab, boardContents.transform);
                newPanel.transform.GetComponentInChildren<TMP_Text>().text = data.data[i].content;
            }
        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }
    [Serializable]
    private struct CommentResInfo
    {
        public int commentId;
        public string content;
        public int boardId;
    }
    [Serializable]
    private struct CommentList
    {
        public CommentResInfo[] data;
    }

    [Serializable]
    private struct BoardGetList
    {
        public List<BoardGetInfo> data;
    }

    [Serializable]
    private struct BoardGetInfo
    {
        public int boardId;
        public string title;
        public string content;
        public int likeCount;
    }
}
