using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {

    protected float speed = 20; // Gotta go fast
    protected BaseEnemy _target;
    protected bool _isUsed = true;

    public void Launch(BaseEnemy enemy)
    {
        _target = enemy;
    }

    public void Explode()
    {
        // TODO Do damage
        ProjectilePool pool = ProjectilePool.GetInstance();
        pool.DestroyProjectile(this);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // TODO get the position of the enemy and go in its direction
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
    static public String GetProjectileType()
    {
        return "BaseProjectile";
    }

    // Need to be override
    public bool IsSameType(BaseProjectile projectile)
    {
        return false;
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
}
