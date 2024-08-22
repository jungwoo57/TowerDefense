using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private Transform[] wayPoints;

    public Transform canvasTransform;
    public GameObject enemyHpSliderPrefab;
    public PlayerHp playerHp;
    public PlayerGold playerGold;
    private List<Enemy> enemyList;
    private int currentEnemyCount;
    private Wave currentWave;
    public List<Enemy> EnemyList => enemyList;
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;


    private void Awake()
    {
        enemyList = new List<Enemy>();
        //StartCoroutine("SpawnEnemy");
    }
    public void StartWave(Wave wave) {
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy() 
    {
        int spawnEnemyCount = 0;
        while (spawnEnemyCount < currentWave.maxEnemyCount) 
        {
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();

            enemy.Setup(this,wayPoints);
            enemyList.Add(enemy);
            SpawnEnemyHpSlider(clone);
            spawnEnemyCount++;
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold) {
        if (type == EnemyDestroyType.Arrive)
        {
            playerHp.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.Kill) {
            playerGold.CurrentGold += gold;
        }
        currentEnemyCount--;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    void SpawnEnemyHpSlider(GameObject enemy) {
        GameObject sliderClone = Instantiate(enemyHpSliderPrefab);
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;
        sliderClone.GetComponent<SilderPositionAutoSetter>().SetUp(enemy.transform);
        sliderClone.GetComponent<EnemyHpViewer>().SetUp(enemy.GetComponent<EnemyHp>());
    }
}
