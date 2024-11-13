using Photon.Realtime;
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
    public string roomPassword = "99999";
    //룸 패스워드 인풋 필드
    public TMP_InputField passWordInputField;

    //룸 패스워드 안내 텍스트
    public TMP_Text passWordText;

    // 패스워드 틀렸을 때 텍스트
    public TMP_Text passWordWrongText;

    //패스워드 제출 버튼
    public Button passWordSubmitButton;
    public Button passWordExitButton;

    private BoxCollider2D boxCollider;

    private bool activeRoom = false;

    void Start()
    {
        passWordPanel.SetActive(false);
        passWordSubmitButton.onClick.AddListener(PassWordCheck);
        passWordExitButton.onClick.AddListener(PassWordExit);
        boxCollider = GetComponent<BoxCollider2D>();
        passWordWrongText.enabled = false;

    }

    void Update()
    {

    }

    private void OnUI(GameObject player)
    {
        passWordInputField.text = "";
        passWordPanel.SetActive(true);

        if (activeRoom == false)
        {
            passWordText.text = "비밀번호 설정해주세요";
        }
        else
        {
            passWordText.text = "비밀번호를 입력해주세요";
        }
    }

    public void PassWordCheck()
    {
        if (!activeRoom)
        {
            roomPassword = passWordInputField.text;
            boxCollider.isTrigger = true;
            passWordPanel.SetActive(false);
            activeRoom = true;
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
                passWordWrongText.gameObject.SetActive(true);
            }

        }

    }

    public void PassWordExit()
    {
        passWordPanel.SetActive(false);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnUI(collision.gameObject);
            playersList.Add(collision.gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
          //  playersList.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for(int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i] == collision.gameObject)
                {
                    playersList.RemoveAt(i);
                    boxCollider.isTrigger = false;
                    break;
                }
            }
            if(playersList.Count == 0)
            {
                activeRoom = false;
            }

        }
    }
}
