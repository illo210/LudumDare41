﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : BaseEntity
{
    protected List<BaseEntity> _inRange = new List<BaseEntity>();
    protected BaseEntity _oldTower;

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        
        Move(move);
    }

    void Update()
    {
        int input = 0;
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            if (CanJump())
            {
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            input = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            input = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            input = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            input = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            input = 5;
        if (input != 0)
        {
            if (gameManager.Gold > gameManager.BaseTowerList[input - 1].GetLevelUpPrice())
            {
                GameObject newTower = Instantiate(gameManager.BaseTowerList[input - 1].gameObject);
                newTower.transform.position = transform.position;
                gameManager.Gold -= gameManager.BaseTowerList[input - 1].GetLevelUpPrice();
            }
        }

        BaseEntity tower = GetActiveTurret();
        if (_oldTower && tower != _oldTower)
            _oldTower.DeactiveHiglight();
        _oldTower = null;
        if (tower)
        {
            if (tower != _oldTower)
                tower.ActiveHiglight();
            BaseTower lvl = tower.GetComponent<BaseTower>();
            gameManager.Upgrade = lvl.GetLevelUpPrice();
            gameManager.Sell = lvl.GetSellPrice();

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                if (lvl.GetLevelUpPrice() != -1 && gameManager.Gold >= lvl.GetLevelUpPrice())
                {
                    gameManager.Gold -= lvl.GetLevelUpPrice();
                    lvl.LevelUp();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                gameManager.Gold += lvl.GetSellPrice();
                Destroy(lvl.gameObject);
            }
            else
            {
                _oldTower = tower;
            }
        }
        else
        {
            gameManager.Upgrade = 0;
            gameManager.Sell = 0;
        }
    }

    protected override void GetHealthBar()
    {
        return;
    }

    public override bool CanBeTarget(BaseProjectile projectile)
    {
        return false;
    }

    protected BaseEntity GetActiveTurret()
    {
        if (_inRange.Count <= 0)
            return null;
        _inRange.RemoveAll(item => item == null);
        _inRange.Sort((p1, p2) => Vector3.Distance(p1.transform.position, transform.position)
            .CompareTo(Vector3.Distance(p2.transform.position, transform.position)));
        return _inRange[0];
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Tower"))
        {
            _inRange.Add(other.gameObject.GetComponent<BaseEntity>());
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Tower"))
        {
            BaseEntity tower = other.gameObject.GetComponent<BaseEntity>();
            if (_inRange.Contains(tower))
            {
                _inRange.Remove(tower);
            }
        }
    }
}