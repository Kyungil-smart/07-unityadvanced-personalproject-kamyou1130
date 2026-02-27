using UnityEngine;

public class MonsterData
{
    private int _monsterHp = 200;
    public int MonsterHp
    {
        get => _monsterHp;
        set
        {
            _monsterHp = value;
        }
    }
}
