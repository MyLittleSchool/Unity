using GH;
using MJ;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ROCK;

public class RockColor : MonoBehaviour
{
    public ROCKCOLOR rockColor;
    private SpriteRenderer spriteRenderer;
    private InputRocks rocksData1;
    private InputRocks rocksData2;

    bool bCollision;

    LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rocksData1 = GameObject.Find("RockManager1").GetComponentInChildren<InputRocks>();
        rocksData2 = GameObject.Find("RockManager2").GetComponentInChildren<InputRocks>();

        InitColor();
    }

    // Update is called once per frame
    void Update()
    {
        KeyInputCheck();

        Debug.Log("bCollision: " + bCollision);
    }

    public void InitColor()
    {
        SetColor(rockColor);
    }

    public void SetColor(ROCKCOLOR _rockColor)
    {
        rockColor = _rockColor;
        Color newColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        switch (_rockColor)
        {
            case ROCKCOLOR.NONE:
                spriteRenderer.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                break;
            case ROCKCOLOR.WHITE:
                spriteRenderer.color = Color.white;
                break;
            case ROCKCOLOR.PURPLE:
                ColorUtility.TryParseHtmlString("#6F4BF2", out newColor);
                break;
            case ROCKCOLOR.YELLOW:
                ColorUtility.TryParseHtmlString("#DFF24B", out newColor);
                break;
            case ROCKCOLOR.GREEN:
                ColorUtility.TryParseHtmlString("#80F2BD", out newColor);
                break;
            case ROCKCOLOR.ORANGE:
                ColorUtility.TryParseHtmlString("#F28B50", out newColor);
                break;
            case ROCKCOLOR.RED:
                ColorUtility.TryParseHtmlString("#F23D3D", out newColor);
                break;
        }
        spriteRenderer.color = newColor;
    }

    public void KeyInputCheck()
    {
        if (gameInteractButton.GetInstance().GetButtonDown())
            CheckColorTile();
    }

    public void ChangeRockColor()
    {
        rocksData1.SetRockColor(rockColor);
        rocksData2.SetRockColor(rockColor);
    }

    public void CheckColorTile()
    {
        if(bCollision)
            ChangeRockColor();


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.name.Contains("Player") && collisionObject.GetComponentInParent<PhotonView>().IsMine)
            bCollision = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.name.Contains("Player") && collisionObject.GetComponentInParent<PhotonView>().IsMine)
            bCollision = false;
    }

}
