using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MJ
{
    public class FileManager : MonoBehaviour
    {
        private static FileManager instance;
        // Start is called before the first frame update

        private void Awake()
        {
            if (null == instance)
            {
                instance = this;

                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public static FileManager Instance
        {
            get
            {
                if (null == instance)
                    return null;
                return instance;
            }
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // 특정 경로, 형식의 파일 이름을 구하는 함수
        public FileInfo[] GetFileInfo(string filePath, string form = "")
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

            return directoryInfo.GetFiles("*." + form);
        }


    }
}
    