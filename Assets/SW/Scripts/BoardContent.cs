using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SW {
    public class BoardContent : MonoBehaviour
    {
        public int id;
        public string title;
        public string content;
        public int like;
        public int comentCount;
        public bool isExistLike;
        public TMP_Text text;
        public TMP_Text likeCountText;
        public TMP_Text comentCountText;
        public Button button;
    }
}