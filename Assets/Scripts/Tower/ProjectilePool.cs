using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    protected static ProjectilePool _sharedInstance;
    public List<KeyValuePair<BaseProjectile, int>> projectiles;
    public List<BaseProjectile> projectileList;
    public Vector3 spawnPosition;

    static public ProjectilePool GetInstance()
    {
        return _sharedInstance;
    }

    private void Awake()
    {
        _sharedInstance = this;
    }

    public void DestroyProjectile(BaseProjectile projectile)
    {
        projectile.Free();
        projectile.MoveTo(spawnPosition);
    }

    public BaseProjectile GetProjectile(BaseProjectile projectileTemplate)
    {
        foreach (BaseProjectile projectile in projectileList)
        {
            if (projectileTemplate.IsSameType(projectile) && projectile.IsFree())
            {
                projectile.Use();
                return projectile;
            }
        }
        BaseProjectile newProjectile = BaseProjectile.Copy();
        projectileList.Add(newProjectile);
        newProjectile.Use();
        return newProjectile;
    }
}
