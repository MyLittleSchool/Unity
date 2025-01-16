using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SW
{
    public class FriendPanel : MonoBehaviour
    {
        public int id;
        public int friendshipId;
        public UserImage ProfileImage;
        public TMP_Text NickNameText;
        public TMP_Text SimilarityText;
        public TMP_Text StateText;
        public TMP_Text RecommandText;
        public TMP_Text GradeText;
        public TMP_Text locationText;
        public TMP_Text InterestText;
        public TMP_Text MessageText;
        public Button PassButton;
        public Button RequestButton;
        public Button reportButton;
    }
}