using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    protected static ProjectilePool _sharedInstance;
    public List<GameObject> projectiles;
    public List<int> projectilesNumber;
    public List<BaseProjectile> projectileList = new List<BaseProjectile>();
    public Vector3 spawnPosition;

    static public ProjectilePool GetInstance()
    {
        return _sharedInstance;
    }

    private void Awake()
    {
        _sharedInstance = this;
    }

    protected void Start()
    {
        for (int i = 0; i < projectiles.Count; i += 1)
        {
            for (int j = 0; j < projectilesNumber[i]; j += 1)
            {
                GameObject projectile = Instantiate(projectiles[i]);
                projectile.transform.position = spawnPosition;
                projectile.transform.SetParent(transform);
                projectileList.Add(projectile.GetComponent<BaseProjectile>());
            }
        }
    }

    public void DestroyProjectile(BaseProjectile projectile)
    {
        projectile.Free();
        projectile.transform.SetParent(transform);
        projectile.MoveTo(spawnPosition);
    }

    public BaseProjectile GetProjectile(string projectileTemplate)
    {
        BaseProjectile tmp = null;

        foreach (BaseProjectile projectile in projectileList)
        {
            if (projectile.IsSameType(projectileTemplate))
            {
                tmp = projectile;
                if (projectile.IsFree())
                {
                    projectile.Use();
                    return projectile;
                }
            }
        }
        BaseProjectile newProjectile = Instantiate(tmp);
        projectileList.Add(newProjectile);
        newProjectile.Use();
        return newProjectile;
    }
}
