using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROCK : MonoBehaviour
{
    public enum ROCKCOLOR
    {
        NONE,
        WHITE,
        RED,
        YELLOW,
        GREEN,
        BLUE,
        BLACK,
        ROCKDATA_END
    }

    public ROCKCOLOR rockColor;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        SetColor(ROCKCOLOR.NONE);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public ROCKCOLOR GetColor()
    {
        return rockColor;
    }

}
