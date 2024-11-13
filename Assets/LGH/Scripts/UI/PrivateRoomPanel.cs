using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrivateRoomPanel : MonoBehaviour
{
    //룸 패스워드 인풋 필드
    public TMP_InputField passWordInputField;

    //룸 패스워드 안내 텍스트
    public TMP_Text passWordText;

    // 패스워드 틀렸을 때 텍스트
    public TMP_Text passWordWrongText;

    //패스워드 제출 버튼
    public Button passWordSubmitButton;
    public Button passWordExitButton;
    void Start()
    {
    }

    void Update()
    {
        
    }
}
