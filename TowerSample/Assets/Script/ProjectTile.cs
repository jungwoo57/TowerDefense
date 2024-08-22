using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    private EnemyMove movement2D;
    private Transform target;
    private float dmg;

    public void SetUp(Transform target, float attackDmg) {
        movement2D = GetComponent<EnemyMove>();
        this.target = target;
        this.dmg = attackDmg;
    }

    private void Update()
    {
        if (target != null) {
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if (collision.transform != target) return;

        collision.GetComponent<EnemyHp>().TakeDamage(dmg);
        Destroy(gameObject);
    }
}

