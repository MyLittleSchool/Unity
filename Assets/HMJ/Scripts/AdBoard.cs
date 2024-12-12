using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdBoard : MonoBehaviour
{
    public string url;
    // Update is called once per frame
    void Update()
    {
        DesignClick();
    }

    public void DesignClick()
    {
        if (Input.GetMouseButtonDown(0))  // 마우스 클릭이 발생한 경우
        {
            // 마우스 위치 -> 월드 좌표
            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Pixel Perfect Camera의 픽셀 크기 보정
            worldMousePosition.x = Mathf.Floor(worldMousePosition.x * 100f) / 100f;
            worldMousePosition.y = Mathf.Floor(worldMousePosition.y * 100f) / 100f;

            RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.name == gameObject.name)
                PrivateButton();
        }
    }
    public void PrivateButton()
    {
        Application.OpenURL(url);
    }
}
