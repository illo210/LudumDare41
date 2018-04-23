using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    protected float speed = 3.0f; // Gotta go fast
    protected bool _isUsed = false;
    protected BaseEntity _target;

    public void Launch(BaseEntity enemy)
    {
        _target = enemy;
    }

    public void Explode()
    {
        _target.decrease(1);
        _target = null;
        ProjectilePool pool = ProjectilePool.GetInstance();
        pool.DestroyProjectile(this);
    }

    void FixedUpdate()
    {
        if (_isUsed)
        {
            if (!_target._isAlive)
            {
                _target = null;
                ProjectilePool pool = ProjectilePool.GetInstance();
                pool.DestroyProjectile(this);
            }

            if (_target)
            {
                float distance = Vector3.Distance(transform.position, _target.gameObject.transform.position);
                transform.position =
                    Vector3.MoveTowards(transform.position, _target.gameObject.transform.position, speed / distance);
            }
        }
    }

    public void Free()
    {
        _isUsed = false;
    }

    public void MoveTo(Vector3 position)
    {
        transform.position = position;
    }

    // Need to be override
    public String GetProjectileType()
    {
        return "BaseProjectile";
    }

    // Need to be override
    public bool IsSameType(string projectile)
    {
        return GetProjectileType() == projectile;
    }

    public bool IsFree()
    {
        return !_isUsed;
    }

    public void Use()
    {
        _isUsed = true;
    }

    public static BaseProjectile Copy()
    {
        return new BaseProjectile();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (_target && other.gameObject == _target.gameObject)
        {
            Explode();
        }
    }
}