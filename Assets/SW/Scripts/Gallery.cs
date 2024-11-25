using System;
using System.Collections;
using System.Collections.Generic;
using GH;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
namespace SW
{
    public class Gallery : MonoBehaviour
    {
        public Image loadedImg;
        public TMP_InputField titleText;
        private string path;
        public Transform content;
        public GameObject contentPrefab;
        public void FileBrowser()
        {
            string[] ex = new string[]
            {
                "xbm", "tif", "jfif", "ico", "tiff", "svg", "jpeg", "svgz", "jpg", "webp", "png", "bmp", "pjp", "apng", "pjpeg", "avif"
            };
            var paths = StandaloneFileBrowser.OpenFilePanel("열기", "", new ExtensionFilter[1] { new ExtensionFilter("이미지 파일", ex) }, false);
            if (paths.Length > 0)
            {
                byte[] fileData = System.IO.File.ReadAllBytes(paths[0]);
                path = paths[0];
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(fileData))
                {
                    Texture2D loadTexture = texture;
                    loadedImg.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    loadButton.SetActive(false);
                    ChangedContents();
                }
                else return;
            }
            else return;
        }
        public GameObject createPanel;
        public GameObject loadButton;
        public Button saveButton;
        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
        public void SetCreatePanel(bool value)
        {
            if (value)
            {
                createPanel.SetActive(true);
                loadedImg.sprite = null;
                titleText.text = "";
                loadButton.SetActive(true);
                saveButton.interactable = false;
            }
            else
            {
                createPanel.SetActive(false);
            }
        }
        public void ChangedContents()
        {
            if (titleText.text != "" && loadedImg.sprite != null)
            {
                saveButton.interactable = true;
            }
            else
            {
                saveButton.interactable = false;
            }
        }
        public void Save()
        {
            SaveInfo saveInfo = new SaveInfo();
            saveInfo.title = titleText.text;
            saveInfo.schoolId = DataManager.instance.mapId;
            // 통신
            HttpManager httpManager = HttpManager.GetInstance();
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = httpManager.SERVER_ADRESS + "/gallery/upload-image";
            info.contentType = "multipart/form-data";
            info.body = path;
            info.onComplete = (DownloadHandler res) =>
            {
                saveInfo.imgUrl = res.text;
                HttpManager.HttpInfo info2 = new HttpManager.HttpInfo();
                info2.url = httpManager.SERVER_ADRESS + "/gallery";
                info2.contentType = "application/json";
                info2.body = JsonUtility.ToJson(saveInfo);
                info2.onComplete = (DownloadHandler res2) =>
                {
                    LoadData();
                    ToastMessage.OnMessage("등록이 완료되었습니다");
                };
                StartCoroutine(httpManager.Post(info2));
            };
            StartCoroutine(httpManager.UploadFileByFormData(info, "userId_" + AuthManager.GetInstance().userAuthData.userInfo.id + "_" + DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            SetCreatePanel(false);
        }
        [Serializable]
        private struct SaveInfo
        {
            public string imgUrl;
            public string title;
            public int schoolId;
        }
        public void LoadData()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
            HttpManager.HttpInfo info = new HttpManager.HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/gallery/list/" + DataManager.instance.mapId;
            info.onComplete = (DownloadHandler res) =>
            {
                LoadList data = JsonUtility.FromJson<LoadList>("{\"data\":" + res.text + "}");
                for (int i = data.data.Count - 1; i >= 0; i--)
                {
                    GameObject newPanel = Instantiate(contentPrefab, content);
                    GalleryContentPanel comp = newPanel.GetComponent<GalleryContentPanel>();
                    comp.title.text = data.data[i].title;
                    StartCoroutine(DownloadImage(data.data[i].imgUrl, comp.image));
                }
            };
            StartCoroutine(HttpManager.GetInstance().Get(info));
        }
        private IEnumerator DownloadImage(string url, Image uiImage)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                // 요청 전송
                yield return webRequest.SendWebRequest();

                // 에러 처리
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"이미지 다운로드 실패: {webRequest.error}");
                }
                else
                {
                    // 다운로드 성공 - Texture2D로 변환
                    Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);

                    // Texture2D를 Sprite로 변환
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    // UI Image에 Sprite 적용
                    uiImage.sprite = sprite;

                    Debug.Log("이미지 적용 완료");
                }
            }
        }
        [Serializable]
        private struct LoadInfo
        {
            public int id;
            public string imgUrl;
            public string title;
            public int schoolId;
            public string enteredDate;
        }
        [Serializable]
        private struct LoadList
        {
            public List<LoadInfo> data;
        }
    }
}