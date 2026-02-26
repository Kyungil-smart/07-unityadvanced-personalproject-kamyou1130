using UnityEngine;

public class MonsterData
{
    private int _monsterHp = 100;
    public int MonsterHp
    {
        get => _monsterHp;
        set
        {
            _monsterHp = value;
        }
    }
}
