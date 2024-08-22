using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser}
public enum WeaponType { Cannon = 0, Laser, Slow, Buff}
public class Bullet : MonoBehaviour
{
    [Header("Commons")]
    public TowerTemplete towerTemplete;
    public WeaponType weaponType;
    public Transform spawnPoint;
    

    [Header("Cannon")]
    public GameObject projectTilePrefab;

    [Header("Laser")]
    public LineRenderer lineRenderer;
    public Transform hitEffect;
    public LayerMask targetLayer;


    public int level;
    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private EnemySpawn enemySpawn;
    private SpriteRenderer spriteRenderer;
    private PlayerGold playerGold;
    private Tile ownerTile;
    private TowerSpawn towerSpawn;
    private float addedDamage;
    private int buffLevel;
    public Sprite TowerSprite => towerTemplete.weapon[level].sprite;
    public float Damage => towerTemplete.weapon[level].damage;
    public float Rate => towerTemplete.weapon[level].rate;
    public float Range => towerTemplete.weapon[level].range;
    public int maxLevel => towerTemplete.weapon.Length;
    public float Slow => towerTemplete.weapon[level].slow;
    public WeaponType WeaponType => weaponType;
    public float Buff => towerTemplete.weapon[level].buff;
    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public int Bufflevel
    {
        set => buffLevel = Mathf.Max(0, value);
        get => buffLevel;
    }
    public void SetUp(TowerSpawn towerSpawn, EnemySpawn enemySpawn, PlayerGold playerGold, Tile ownerTile) 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.towerSpawn = towerSpawn;
        this.enemySpawn = enemySpawn;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;
        if (weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser)
        {
            ChangeState(WeaponState.SearchTarget);
        }
    }
    
    public void ChangeState(WeaponState newState) 
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }
    private void Update()
    {
        if (attackTarget != null)
            RotateToTarget();
    }
    private void RotateToTarget() 
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }
    private IEnumerator SearchTarget() 
    {
        while (true) {
            attackTarget = FindClosesAttackTarget();
            if (attackTarget != null)
            {
                if(weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                if (weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
            }

            yield return null;
        }
    }
    private IEnumerator TryAttackCannon() {
        while (true) {
            if(IsPossibleAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            yield return new WaitForSeconds(towerTemplete.weapon[level].rate);
            SpawnProjectile();
        }
    }
    private IEnumerator TryAttackLaser()
    {
        EnableLaser();
        while(true)
        {
            if(IsPossibleAttackTarget() == false)
            {
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            SpawnLaser();
            yield return null;
        }
    }

    private Transform FindClosesAttackTarget() 
    {
        float closestDistSqr = Mathf.Infinity;
        for(int i = 0; i < enemySpawn.EnemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawn.EnemyList[i].transform.position, transform.position);
            if(distance <= towerTemplete.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawn.EnemyList[i].transform;
            }
            
        }
        return attackTarget;
    }
    void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }
    void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }
    void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplete.weapon[level].range, targetLayer);

        for(int i = 0; i< hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                lineRenderer.SetPosition(0, spawnPoint.position);
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                hitEffect.position = hit[i].point;
                float damage = towerTemplete.weapon[level].damage + AddedDamage;
                attackTarget.GetComponent<EnemyHp>().TakeDamage(damage * Time.deltaTime);
            }

        }
    }
    private bool IsPossibleAttackTarget()
    {
        if (attackTarget == null)
        {
            return false;
        }
        float distancne = Vector3.Distance(attackTarget.position, transform.position);
        if(distancne > towerTemplete.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }
        
        return true;
    }
    private void SpawnProjectile() {
        GameObject clone = Instantiate(projectTilePrefab, spawnPoint.position, Quaternion.identity);
        float damage = towerTemplete.weapon[level].damage + AddedDamage;
        clone.GetComponent<ProjectTile>().SetUp(attackTarget, damage);
    }
    public void OnBUffAroundTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        for(int i = 0; i < towers.Length; ++i)
        {
            Bullet weapon = towers[i].GetComponent<Bullet>();
            if(weapon.Bufflevel > level+1)
            {
                continue;
            }
            if (Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplete.weapon[level].range) 
            {
                if (weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser)
                {
                    weapon.AddedDamage = weapon.Damage * (towerTemplete.weapon[level].buff);
                    weapon.Bufflevel = level+1;
                }
            }
        }  
    }
    public bool Upgrade()
    {
        if(playerGold.CurrentGold < towerTemplete.weapon[level+1].cost)
        {
            return false;
        }
        level++;
        spriteRenderer.sprite = towerTemplete.weapon[level].sprite;
        playerGold.CurrentGold -= towerTemplete.weapon[level].cost;
        if(weaponType == WeaponType.Laser)
        {
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;

        }
        towerSpawn.OnBuffAllBuffTower();
        return true;
    }
    public void Sell()
    {
        playerGold.CurrentGold += towerTemplete.weapon[level].sell;
        ownerTile.IsBuildTower = false;
        Destroy(gameObject);
    }
}
