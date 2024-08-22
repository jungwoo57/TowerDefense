using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private Bullet towerWeapon;

    private void Awake()
    {
        towerWeapon = GetComponentInParent<Bullet>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }
        EnemyMove movement2D = collision.GetComponent<EnemyMove>();
        movement2D.MoveSpeed -= movement2D.MoveSpeed * towerWeapon.Slow;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }
        collision.GetComponent<EnemyMove>().ResetMoveSpeed();
    }
}
