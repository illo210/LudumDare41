using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour {

    public List<BaseTowerLevel> levelList;
    public List<int> priceList;
    protected int _level;

    public int GetLevelUpPrice()
    {
        if (_level + 1 < priceList.Count)
            return priceList[_level + 1];
        return -1;
    }

    public void LevelUp()
    {
        if (_level + 1 < priceList.Count)
            _level += 1;
    }

    protected void FixedUpdate()
    {
        levelList[_level].TowerUpdate();        
    }

    public BaseTowerLevel getCurrentLevel()
    {
        return levelList[_level];
    }

}
