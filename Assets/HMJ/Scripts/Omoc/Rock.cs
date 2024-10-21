using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROCK : MonoBehaviour
{
    public enum ROCKCOLOR
    {
        NONE,
        WHITE,
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
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case ROCKCOLOR.BLACK:
                spriteRenderer.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                break;
        }
    }

    public ROCKCOLOR GetColor()
    {
        return rockColor;
    }

}
