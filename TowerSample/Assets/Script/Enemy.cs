using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }
public class Enemy : MonoBehaviour
{
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex = 0;
    private EnemyMove movement2D;
    private EnemySpawn  enemySpawner;

    public int gold;
    public void Setup(EnemySpawn enemySpawner, Transform[] wayPoints) 
    {
        movement2D = GetComponent<EnemyMove>();
        this.enemySpawner = enemySpawner;
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        transform.position = wayPoints[currentIndex].position;
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove() 
    {
        NextMoveTo();
        while (true) {
            transform.Rotate(Vector3.forward * 10);

            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed) {
                NextMoveTo();
            }
            yield return null;
        }
    }
    void NextMoveTo()
    {
        if (currentIndex < wayPointCount - 1)
        {
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else {
            gold = 0;
            OnDie(EnemyDestroyType.Arrive); }
    }
    public void OnDie(EnemyDestroyType type) 
    {
        enemySpawner.DestroyEnemy(type,this,gold);
    }
}
