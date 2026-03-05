using UnityEngine;

public class MonsterData
{
    private int _currentMonsterHp;
    public int CurrentMonsterHp
    {
        get => _currentMonsterHp;
        set
        {
            _currentMonsterHp = value;
        }
    }
}
