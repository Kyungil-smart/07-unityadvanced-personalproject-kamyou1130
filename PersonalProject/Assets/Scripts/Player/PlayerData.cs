using UnityEngine;

public class PlayerData
{
    private int _playerHp = 100;
    public int PlayerHp
    {
        get => _playerHp;
        set
        {
            _playerHp = value;
        }
    }

    private int _playerAttackDamage = 20;
    public int PlayerAttackDamage
    {
        get => _playerAttackDamage;
        set
        {
            _playerAttackDamage = value;   
        }
    }

    private float _currentDashCooltime = 0f;
    public float CurrentDashCooltime
    {
        get => _currentDashCooltime;
        set
        {
            _currentDashCooltime = value;
        }
    }
}
