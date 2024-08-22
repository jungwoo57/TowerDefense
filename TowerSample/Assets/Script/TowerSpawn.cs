using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class TowerSpawn : MonoBehaviour
{
    public TowerTemplete[] towerTemplete;
    private int towerType;
    public EnemySpawn enemySpawner;
    public PlayerGold playerGold;
    public SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private GameObject followTowerClone = null;
    public void ReadyToSpawnTower(int type)
    {
        towerType = type;
        if (isOnTowerButton == true)
            return;
        if (towerTemplete[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        isOnTowerButton = true;
        followTowerClone = Instantiate(towerTemplete[towerType].followTowerPrefab);
        StartCoroutine("OnTowerCanCelSystem");
    }
    public void SpawnTower(Transform tileTransform)
    {
        if (isOnTowerButton == false)
            return;

        Tile tile = tileTransform.GetComponent<Tile>();
        if (tile.IsBuildTower == true)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        isOnTowerButton = false;
        tile.IsBuildTower = true;
        playerGold.CurrentGold -= towerTemplete[towerType].weapon[0].cost;
        Vector3 position = tileTransform.position + Vector3.back;

        GameObject clone = Instantiate(towerTemplete[towerType].towerPrefab, position, Quaternion.identity);
        clone.GetComponent<Bullet>().SetUp(this, enemySpawner, playerGold, tile);
        OnBuffAllBuffTower();

        Destroy(followTowerClone);
        StopCoroutine("OnTowerCancelSystem");
    }
    public void OnBuffAllBuffTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        for (int i = 0; i < towers.Length; ++i)
        {
            Bullet weapon = towers[i].GetComponent<Bullet>();
            if (weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBUffAroundTower();
            }
        }
    }
        IEnumerator OnTowerCanCelSystem()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                {
                    isOnTowerButton = false;
                    Destroy(followTowerClone);
                    break;
                }
                yield return null;
            }
        }
    
}
