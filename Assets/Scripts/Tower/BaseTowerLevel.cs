using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerLevel : MonoBehaviour {

    protected enum EnemiesOrder
    {
        StrongerFirst,
        WeakestFirst,
        NearestFirst,
        FarthestFirst,
        First,
        Last,
        End
    }

    public BaseProjectile projectile;
    public ProjectilePool projectilePool;
    protected bool _canAttack = true;
    protected EnemiesOrder _attackOrder;
    protected List<BaseEnemy> _inRange;
    protected SphereCollider _range;

    protected void Start()
    {
        _range = GetComponent<SphereCollider>();
    }

    protected float GetCooldown()
    {
        return 1.0f;
    }

    private IEnumerator AttackCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        _canAttack = true;
    }

    protected List<BaseEnemy> OrderEnemies(List<BaseEnemy> enemies)
    {
        return enemies;
    }

    protected void Attack(List<BaseEnemy> enemies)
    {
        if (_canAttack)
        {
            _canAttack = false;
            BaseProjectile newProjectile = projectilePool.GetProjectile(projectile);
            newProjectile.MoveTo(transform.position);
            newProjectile.Launch(enemies[0]);
            StartCoroutine(AttackCooldown(GetCooldown()));
        }
    }

    protected List<BaseEnemy> GetEnemiesInRange()
    {
        return OrderEnemies(_inRange);
    }

    protected List<EnemiesOrder> GetEnemiesOrder()
    {
        List<EnemiesOrder> ret = new List<EnemiesOrder>
        {
            EnemiesOrder.StrongerFirst,
            EnemiesOrder.WeakestFirst,
            EnemiesOrder.NearestFirst,
            EnemiesOrder.FarthestFirst,
            EnemiesOrder.First,
            EnemiesOrder.Last,
        };

        return ret;
    }

    protected EnemiesOrder GetAttackOrder()
    {
        return _attackOrder;
    }

    protected void NextEnemiesOrder()
    {
        _attackOrder = (_attackOrder + 1);
        if (_attackOrder == EnemiesOrder.End)
            _attackOrder = 0;
    }

    protected void PrevEnemiesOrder()
    {
        _attackOrder = (_attackOrder - 1);
        if (_attackOrder < 0)
            _attackOrder = EnemiesOrder.End - 1;
    }

    public void TowerUpdate()
    {
        List<BaseEnemy> enemies = GetEnemiesInRange();
        if (enemies.Count > 0)
        {
            Attack(enemies);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        BaseEnemy enemy = other.GetComponentInParent<BaseEnemy>();
        // TODO check if we can target the enemy
        _inRange.Add(enemy);
    }

    protected void OnTriggerExit(Collider other)
    {
        BaseEnemy enemy = other.GetComponentInParent<BaseEnemy>();
        if (_inRange.Contains(enemy))
        {
            _inRange.Remove(enemy);
        }
    }
}
