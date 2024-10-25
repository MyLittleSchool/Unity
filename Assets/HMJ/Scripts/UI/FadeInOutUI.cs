using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutUI : MonoBehaviour
{
    Image[] fadeUIImage;
    TMP_Text[] fadeTextUI;
    public float fadeInDuration = 1.0f;
    public float fadeOutDuration = 1.0f;

    private void Start()
    {
        fadeUIImage = GetComponentsInChildren<Image>();
        fadeTextUI = GetComponentsInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        
    }

    public void FadeOut(float delayTime)
    {
        StartCoroutine(FadeOutRoutine(delayTime));
    }

    public void FadeIn(float delayTime)
    {
        StartCoroutine(FadeInRoutine(delayTime));
    }

    public void FadeInOut(float fadeOutDelayTime, float fadeInDelayTime)
    {
        StartCoroutine(FadeOutRoutine(fadeOutDelayTime));
        StartCoroutine(FadeInRoutine(fadeInDelayTime));
    }
    private IEnumerator FadeOutRoutine(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);

        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;

            foreach (TMP_Text text in fadeTextUI)
            {
                Color color = text.color;
                color.a = elapsedTime / fadeInDuration;
                text.color = color;
            }

            foreach (Image image in fadeUIImage)
            {
                Color color = image.color;
                color.a = elapsedTime / fadeInDuration;
                image.color = color;
            }
            yield return null;
        }

    }

    private IEnumerator FadeInRoutine(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);

        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;

            foreach (TMP_Text text in fadeTextUI)
            {
                Color color = text.color;
                color.a = 1 - elapsedTime / fadeOutDuration;
                text.color = color;
            }

            foreach (Image image in fadeUIImage)
            {
                Color color = image.color;
                color.a = 1 - elapsedTime / fadeOutDuration;
                image.color = color;
            }
            yield return null;
        }
    }
}
