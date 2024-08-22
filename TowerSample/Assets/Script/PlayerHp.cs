using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHp : MonoBehaviour
{
    public Image imagescreen;
    public float maxHp;
    private float currentHp;

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp == 0)
            Debug.Log("플레이어의 체력이 0입니다");

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");
    }
    IEnumerator HitAlphaAnimation()
    {
        Color color = imagescreen.color;
        color.a = 0.5f;
        imagescreen.color = color;

        while (color.a >= 0.0f) {
            color.a -= Time.deltaTime;
            imagescreen.color = color;
            yield return null;
        }
    }
}
