using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public Wave[] waves;
    public EnemySpawn enemySpawner;
    private int currentWaveIndex = -1;

    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;
    public void StartWave()
    {
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }

    }
}

[System.Serializable]
public struct Wave {
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;

}
