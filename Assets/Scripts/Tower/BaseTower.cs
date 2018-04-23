using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : BaseEntity {

    public List<GameObject> levelList;
    public List<int> priceList;
    protected int _level = 0;
    protected GameObject _tower;

    protected override void Start()
    {
        base.Start();
        _tower = Instantiate(levelList[0]);
        _tower.transform.position = transform.position;
        _tower.transform.SetParent(transform);
    }

    public int GetLevelUpPrice()
    {
        if (_level < priceList.Count)
            return priceList[_level];
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
}
