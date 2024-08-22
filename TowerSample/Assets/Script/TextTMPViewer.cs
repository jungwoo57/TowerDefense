using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTMPViewer : MonoBehaviour
{
    public TextMeshProUGUI textPlayerHp;
    public PlayerHp playerHp;
    public TextMeshProUGUI textGold;
    public PlayerGold playerGold;
    public TextMeshProUGUI textWave;
    public WaveSystem waveSystem;
    public TextMeshProUGUI enemyText;
    public EnemySpawn enemySpawner;
    private void Update()
    {
        textPlayerHp.text = playerHp.CurrentHp + "/" + playerHp.MaxHp;
        textGold.text = playerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
        enemyText.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
    }
}
