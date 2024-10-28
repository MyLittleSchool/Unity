using GH;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    [Header("·Î±×ÀÎ ¾À")]
    public TMP_InputField nameInputField;
    public TMP_InputField schoolInputField;
    void Start()
    {
        DontDestroyOnLoad(gameObject);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Login()
    {
        DataManager.instance.playerName = nameInputField.text;
        DataManager.instance.playerSchool = schoolInputField.text;

        SceneManager.LoadScene(1);
    }
}
