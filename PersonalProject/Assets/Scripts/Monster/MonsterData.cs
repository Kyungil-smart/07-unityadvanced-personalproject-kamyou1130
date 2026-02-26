using UnityEngine;

public class MonsterData
{
    private int _monsterHp = 50;
    public int MonsterHp
    {
        get => _monsterHp;
        set
        {
            _monsterHp = value;
        }
    }
}
