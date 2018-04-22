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
    protected ProjectilePool _projectilePool;
    protected bool _canAttack = true;
    protected EnemiesOrder _attackOrder;
    protected List<GameObject> _inRange = new List<GameObject>();
    protected SphereCollider _range;

    protected virtual void Start()
    {
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

    protected List<GameObject> OrderEnemies(List<GameObject> enemies)
    {
        return enemies;
    }

    protected void Attack(List<GameObject> enemies)
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

    protected List<GameObject> GetEnemiesInRange()
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

    public void FixedUpdate()
    {
        List<GameObject> enemies = GetEnemiesInRange();
        if (enemies.Count > 0)
        {
            Attack(enemies);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Entity"))
        {
            BaseEntity enemy = other.gameObject.GetComponentInParent<BaseEntity>();
            if (enemy.CanBeTarget(projectile))
                _inRange.Add(other.gameObject);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        GameObject enemy = other.gameObject;
        if (_inRange.Contains(enemy))
        {
            _inRange.Remove(enemy);
        }
    }
}
