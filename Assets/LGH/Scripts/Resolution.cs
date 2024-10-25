using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class Resolution : MonoBehaviour
    {
        public float w = 1080;
        public float h = 1920;

        public float scaleFactor;




        void Start()
        {
            CanvasScaler cs = GetComponent<CanvasScaler>();

            float ratioX = Screen.width / w;
            float ratioY = Screen.height / h;

            if (ratioX < ratioY) cs.scaleFactor = ratioX;
            else cs.scaleFactor = ratioY;

            scaleFactor = cs.scaleFactor;

            //print(Screen.width + ", " + Screen.height);
            //print(Screen.width / cs.scaleFactor + ", " + Screen.height / cs.scaleFactor);
        }

        public Vector3 GetCoord(Vector3 p)
        {
            return p / scaleFactor;
        }

        void Update()
        {

        }
    }

}