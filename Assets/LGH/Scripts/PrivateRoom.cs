using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrivateRoom : MonoBehaviour
{
    public List<GameObject> playersList = new List<GameObject>();
    public GameObject passWordPanel;

    //룸 패스워드
    public string roomPassword;
    //룸 패스워드 인풋 필드
    public TMP_InputField passWordInputField;

    //룸 패스워드 안내 텍스트
    public TMP_Text passWordText;

    // 패스워드 틀렸을 때 텍스트
    public TMP_Text passWordWrongText;

    //패스워드 제출 버튼
    public Button passWordSubmitButton;

    private BoxCollider2D boxCollider;

    void Start()
    {
        passWordPanel.SetActive(false);
        passWordSubmitButton.onClick.AddListener(PassWordCheck);
        boxCollider = GetComponent<BoxCollider2D>();
        passWordWrongText.enabled = false;

    }

    void Update()
    {

    }

    private void OnUI(GameObject player)
    {
        passWordPanel.SetActive(true);

        if (playersList.Count < 1)
        {
            passWordText.text = "비밀번호 설정";
            playersList.Add(player);
        }
        else
        {
            passWordText.text = "비밀번호";
        }
    }

    public void PassWordCheck()
    {
        if (playersList.Count < 1)
        {
            roomPassword = passWordInputField.text;
            boxCollider.isTrigger = true;
            passWordPanel.SetActive(false);
        }
        else
        {
            if (passWordInputField.text == roomPassword)
            {
                boxCollider.isTrigger = true;
                passWordPanel.SetActive(false);
            }
            else
            {
                passWordWrongText.enabled = true;
            }

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnUI(collision.gameObject);

        }
    }
}
