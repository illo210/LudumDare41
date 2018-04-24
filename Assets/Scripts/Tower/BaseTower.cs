using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : BaseEntity {

    public List<GameObject> levelList;
    public List<int> priceList;
    protected int _level = 0;
    protected GameObject _tower;
    protected Rigidbody _rb;
    protected UnityEngine.UI.Image _baseSprite;

    protected override void Awake()
    {
        base.Awake();
        _tower = Instantiate(levelList[0]);
        _tower.transform.position = transform.position;
        _tower.transform.SetParent(transform);
        _baseSprite = _tower.GetComponent<BaseTowerLevel>().GetSprite();
        _rb = GetComponent<Rigidbody>();
    }

    public int GetLevelUpPrice()
    {
        if (_level + 1 < priceList.Count)
            return priceList[_level + 1];
        return -1;
    }

    public int GetSellPrice()
    {
        if (_level < priceList.Count)
            return priceList[_level] / 2;
        return -1;
    }

    public void LevelUp()
    {
        if (_level + 1 <= priceList.Count)
        {
            _level += 1;
            Destroy(_tower);
            _tower = Instantiate(levelList[_level]);
            _tower.transform.position = transform.position;
            _tower.transform.SetParent(transform);
        }
    }

    protected override void GetHealthBar()
    {
        return;
    }

    public UnityEngine.UI.Image GetSprite()
    {
        return _tower.GetComponent<BaseTowerLevel>().GetSprite();
    }

    public UnityEngine.UI.Image GetBaseSprite()
    {
        return _baseSprite;
    }

    public string GetName()
    {
        return _tower.GetComponent<BaseTowerLevel>().GetName();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        bool tmpGrounded = isGrounded;
        base.OnCollisionEnter(collision);
        if (tmpGrounded == false && isGrounded == true)
            rb.constraints = RigidbodyConstraints.FreezeAll;

        if (isGrounded)
        {
            if (collision.gameObject.tag.Contains("Tower"))
            {
                gameManager.Gold += 5;
                Destroy(collision.gameObject);
            }
        }
    }
}
