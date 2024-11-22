using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROCK : MonoBehaviour
{
    public enum ROCKCOLOR
    {
        NONE,
        WHITE,
        PURPLE,
        YELLOW,
        GREEN,
        ORANGE,
        RED,
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

    public ROCKCOLOR GetColor()
    {
        return rockColor;
    }

}
