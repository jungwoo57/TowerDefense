using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public float currentGold;

    public float CurrentGold {
        set => currentGold = Mathf.Max(0, value);
        get => currentGold;
    }
}
