using GH;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
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

    public List<string> loadingString;
    public Slider loadingSlider;

    public TMP_Text loadingText;

    //애니메이션 실행
    float frameRate = 0.1f;
    int currentFrame = 0;
    float timer = 0f;

    float textTime = 0;
    int textnum = 0;

    void Start()
    {
        loadingSlider = GetComponentInChildren<Slider>();
        StartCoroutine(SceneMove());
    }

    void Update()
    {
        LoadingText();
        LoadingSilde();
        UdateTime();


        SpriteAnimation(clothes[clothesid].sideAnimations, clothesSpriteRenderer);
        SpriteAnimation(skin[skinid].sideAnimations, skinSpriteRenderer);
        SpriteAnimation(face[faceid].sideAnimations, faceSpriteRenderer);
        SpriteAnimation(hair[hairid].sideAnimations, hairSpriteRenderer);

        clothesSpriteRenderer.rectTransform.localScale = new Vector3(1, 1, 1);
        skinSpriteRenderer.rectTransform.localScale = new Vector3(1, 1, 1);
        faceSpriteRenderer.rectTransform.localScale = new Vector3(1, 1, 1);
        hairSpriteRenderer.rectTransform.localScale = new Vector3(1, 1, 1);



    }
    private void RandomSet()
    {
        skinid = Random.Range(0, skin.Count);
        clothesid = Random.Range(0, clothes.Count);
        faceid = Random.Range(0, face.Count);
        hairid = Random.Range(0, hair.Count);
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
            {
                currentFrame = 0;
            }
        }
    }
    private void LoadingText()
    {
        loadingText.text = loadingString[textnum];
        textTime += Time.deltaTime;
        if (textTime > 0.5f)
        {
            textnum++;
            if (textnum >= loadingString.Count)
            {
                textnum = 0;
            }
            textTime = 0;
        }

    }
    float currentTime = 0;
    float maxTime = 3;
    private void LoadingSilde()
    {
        currentTime += Time.deltaTime;
        loadingSlider.value = Mathf.Lerp(0, 1, currentTime / maxTime);
    }
    //public void SceneMove()
    //{
    //    currentTime = 0;
    //    RandomSet();
    //}
    public IEnumerator SceneMove()
    {
        currentTime = 0;
        RandomSet();
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
