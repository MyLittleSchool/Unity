using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterestLayout : MonoBehaviour
{
    GridLayoutGroup group;
    RectTransform rectTransform;
    void Start()
    {
        group = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        //group.cellSize = new Vector2( ((Screen.width - 200) - (group.spacing.x * (group.constraintCount - 1))) / group.constraintCount, group.cellSize.y);
    }
}
