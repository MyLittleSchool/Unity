using GH;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecoPlayerUI : MonoBehaviour
{
    public List<AvatarSprite> skin;
    public List<AvatarSprite> clothes;
    public List<AvatarSprite> face;
    public List<AvatarSprite> hair;

    public Image skinSpriteRenderer;
    public Image clothesSpriteRenderer;
    public Image faceSpriteRenderer;
    public Image hairSpriteRenderer;

    public int skinid;
    public int clothesid;
    public int faceid;
    public int hairid;

    //애니메이션 실행
    float frameRate = 0.1f;
    int currentFrame = 0;
    float timer = 0f;


    void Start()
    {
    }

    void Update()
    {
        UdateTime();


        SpriteAnimation(clothes[clothesid].frontAnimations, clothesSpriteRenderer);
        SpriteAnimation(skin[skinid].frontAnimations, skinSpriteRenderer);
        SpriteAnimation(face[faceid].frontAnimations, faceSpriteRenderer);
        SpriteAnimation(hair[hairid].frontAnimations, hairSpriteRenderer);

        clothesSpriteRenderer.rectTransform.localScale = new Vector3(1, 1, 1);
        skinSpriteRenderer.rectTransform.localScale = new Vector3(1, 1, 1);
        faceSpriteRenderer.rectTransform.localScale = new Vector3(1, 1, 1);
        hairSpriteRenderer.rectTransform.localScale = new Vector3(1, 1, 1);



    }

    public void SetAnimationSpriteIndex(int _skinId, int _clothesId, int _faceId, int _hairId)
    {
        skinid = _skinId;
        clothesid = _clothesId;
        faceid = _faceId;
        hairid = _hairId;
    }

    private void SpriteAnimation(List<Sprite> sprites, Image image)
    {
        image.sprite = sprites[currentFrame];
    }

    private void UdateTime()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0;
            currentFrame++;

            if (currentFrame >= 4)
                currentFrame = 0;
        }
    }


}
