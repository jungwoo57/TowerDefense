using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TMPAlpha : MonoBehaviour
{
    public float lerpTime;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void FadeOut()
    {
        StartCoroutine(AlphaLerp(1,0));
    }

    IEnumerator AlphaLerp(float start, float end) 
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        while(percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / lerpTime;

            Color color = text.color;
            color.a = Mathf.Lerp(start, end, percent);
            text.color = color;

            yield return null;
        }
    }
}
