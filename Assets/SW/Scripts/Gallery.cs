using System.Collections;
using System.Collections.Generic;
using SFB;
using UnityEngine;
using UnityEngine.UI;
namespace SW
{
    public class Gallery : MonoBehaviour
    {
        public Image testObj;
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
                    testObj.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
                else return;
            }
            else return;
        }
    }
}