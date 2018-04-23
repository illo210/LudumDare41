using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : BaseEntity
{
    protected List<BaseEntity> _inRange = new List<BaseEntity>();

    void Update()
    {
        float move = Input.GetAxis("Horizontal");

        Move(move);
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
        {
            if (gameManager.Gold >= 5)
            {
                GameObject newTower = Instantiate(Resources.Load("Prefabs/" + "Tower1") as GameObject);
                newTower.transform.position = transform.position;
                gameManager.Gold -= 5;
            }
        }

        BaseEntity tower = GetActiveTurret();
        if (tower)
        {
            BaseTower lvl = tower.GetComponent<BaseTower>();
            gameManager.Upgrade = lvl.GetLevelUpPrice();
            gameManager.Sell = lvl.GetLevelUpPrice() / 2;

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Debug.Log(lvl.GetLevelUpPrice());
                if (lvl.GetLevelUpPrice() != -1 && gameManager.Gold >= lvl.GetLevelUpPrice())
                {
                    Debug.Log("Upgrade");
                    gameManager.Gold -= lvl.GetLevelUpPrice();
                    lvl.LevelUp();
                }
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