using SW;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace SW
{
    public class Debuger : MonoBehaviour
    {
        public TMP_InputField objId;
        public TMP_InputField x;
        public TMP_InputField y;
        public TMP_InputField rot;
        public TMP_InputField id;

        public TMP_InputField userId;
        public TMP_InputField nickName;
        public TMP_InputField title;
        public TMP_InputField content;

        public Button set;
        public Button get;
        public Button delete;
        public Button create;
        public Button load;

        PostManager postManager;
        PlaceManager placeManager;
        // Start is called before the first frame update
        void Start()
        {
            postManager = GetComponent<PostManager>();
            placeManager = GetComponent<PlaceManager>();
            set.onClick.AddListener(() =>
            {
                PlaceManager.ObjectInfo objectInfo = new PlaceManager.ObjectInfo();
                objectInfo.objId = int.Parse(objId.text);
                objectInfo.x = int.Parse(x.text);
                objectInfo.y = int.Parse(y.text);
                objectInfo.rot = int.Parse(rot.text);
                placeManager.CreatePlace(objectInfo, null);
            });
            get.onClick.AddListener(() =>
            {
                //placeManager.ReadPlace(-1, null);
            });
            delete.onClick.AddListener(() =>
            {
                placeManager.DeletePlace(int.Parse(id.text));
            });
            create.onClick.AddListener(() =>
            {
                //PostManager.PostInfo postInfo = new PostManager.PostInfo();
                //postInfo.userId = int.Parse(userId.text);
                //postInfo.nickname = nickName.text;
                //postInfo.title = title.text;
                //postInfo.content = content.text;
                //postManager.CreatePost(postInfo);
            });
            load.onClick.AddListener(() =>
            {
                //postManager.LoadPost();
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}