using System.Collections;
using System.Collections.Generic;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SW
{
    public class Gallery : MonoBehaviour
    {
        public Image loadedImg;
        public TMP_InputField titleText;
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
            // 통신

            SetCreatePanel(false);
        }
    }
}