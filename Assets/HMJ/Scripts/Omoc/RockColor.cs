using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ROCK;

public class RockColor : MonoBehaviour
{
    public ROCKCOLOR rockColor;
    private SpriteRenderer spriteRenderer;
    private InputRocks rocksData;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rocksData = GameObject.Find("RockManager").GetComponentInChildren<InputRocks>();
        InitColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitColor()
    {
        SetColor(rockColor);
    }

    public void SetColor(ROCKCOLOR _rockColor)
    {
        rockColor = _rockColor;

        switch (_rockColor)
        {
            case ROCKCOLOR.NONE:
                spriteRenderer.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                break;
            case ROCKCOLOR.WHITE:
                spriteRenderer.color = Color.white;
                break;
            case ROCKCOLOR.RED:
                spriteRenderer.color = Color.red;
                break;
            case ROCKCOLOR.YELLOW:
                spriteRenderer.color = Color.yellow;
                break;
            case ROCKCOLOR.GREEN:
                spriteRenderer.color = Color.green;
                break;
            case ROCKCOLOR.BLUE:
                spriteRenderer.color = Color.blue;
                break;
            case ROCKCOLOR.BLACK:
                spriteRenderer.color = Color.black;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Space) && collision.gameObject.name == "Player")
        {
             rocksData.SetRockColor(rockColor);
        }
    }

}
