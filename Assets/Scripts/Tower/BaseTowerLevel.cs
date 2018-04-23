using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerLevel : BaseEntity
{
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
    protected ProjectilePool _projectilePool;
    protected bool _canAttack = true;
    protected EnemiesOrder _attackOrder;
    protected List<BaseEntity> _inRange = new List<BaseEntity>();
    protected SphereCollider _range;

    protected override void Start()
    {
        base.Start();
        _projectilePool = GameObject.Find("ProjectilePool").GetComponent<ProjectilePool>();
        _range = GetComponent<SphereCollider>();
    }

    protected float GetCooldown()
    {
        return 0.5f;
    }

    private IEnumerator AttackCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        _canAttack = true;
    }

    protected List<BaseEntity> OrderEnemies(List<BaseEntity> enemies)
    {
        return enemies;
    }

    protected void Attack(List<BaseEntity> enemies)
    {
        if (_canAttack)
        {
            _canAttack = false;
            BaseProjectile newProjectile = _projectilePool.GetProjectile(projectile.GetProjectileType());
            newProjectile.MoveTo(transform.position);
            newProjectile.transform.SetParent(transform);
            newProjectile.Launch(enemies[0]);
            StartCoroutine(AttackCooldown(GetCooldown()));
        }
    }

    protected List<BaseEntity> GetEnemiesInRange()
    {
        List<BaseEntity> inRange = new List<BaseEntity>();
        foreach (BaseEntity entity in _inRange)
        {
            if (entity && entity._isAlive)
                inRange.Add(entity);
        }

        return OrderEnemies(inRange);
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

    public void FixedUpdate()
    {
        List<BaseEntity> enemies = GetEnemiesInRange();
        if (enemies.Count > 0)
        {
            Attack(enemies);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Entity") && !other.tag.Contains("Tower"))
        {
            BaseEntity enemy = other.gameObject.GetComponentInParent<BaseEntity>();
            if (enemy.CanBeTarget(projectile))
                _inRange.Add(other.gameObject.GetComponent<BaseEntity>());
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Entity") && !other.tag.Contains("Tower"))
        {
            BaseEntity enemy = other.gameObject.GetComponent<BaseEntity>();
            if (_inRange.Contains(enemy))
            {
                _inRange.Remove(enemy);
            }
        }
    }
}