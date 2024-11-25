using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TmpItem : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemCount;
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame                       
    void Update()
    {
    }

    public void SetText_Image(string _name, int _count, Texture _texture)
    {
        if(itemName)
            itemName.text = _name;
        if(itemCount)
            itemCount.text = _count.ToString();
        if(rawImage)
            rawImage.texture = _texture;
    }
}
