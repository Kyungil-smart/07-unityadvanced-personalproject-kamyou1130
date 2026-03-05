using UnityEngine;

public class PlayerData
{
    private int _currentPlayerHp = 100;
    public int CurrentPlayerHp
    {
        get => _currentPlayerHp;
        set
        {
            _currentPlayerHp = value;
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

    private float _currentDashCooltime;
    public float CurrentDashCooltime
    {
        get => _currentDashCooltime;
        set
        {
            _currentDashCooltime = value;
        }
    }

    private float _currentBombCooltime;
    public float CurrentBombCooltime
    {
        get => _currentBombCooltime;
        set
        {
            _currentBombCooltime = value;   
        }
    }
}
