using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public enum State
{
    Idle,
    Chase,
    Attack,
    Die
}
public class MonsterController : MonoBehaviour, IDamagable
{
    [SerializeField] private MonsterAI _monsterAI;
    [SerializeField] private Transform _player;
    [SerializeField] private List<MonsterSkill> _monsterSkill;
    
    // Monster 데이터 접근 필드 (MVP 패턴)
    private MonsterData _monsterData;
    
    // FSM 유한 상태 머신 접근 필드
    private State _currentState;
    private float _chaseRange;
    private float _attackRange;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Die:
                break;
        }
    }

    private void Init()
    {
        if (_monsterAI == null) _monsterAI = FindAnyObjectByType<MonsterAI>();
        _monsterData = new MonsterData();
    }

    private void Idle()
    {
        if (IsPlayerInRange(_chaseRange))
        {
            _currentState = State.Chase;
        }
    }

    private void Chase()
    {
        _monsterAI._canMove = true;


        if (_monsterData.MonsterHp <= 0)
        {
            _monsterAI._canMove = false;
            _currentState = State.Die;
        }
        else if (IsPlayerInRange(_attackRange))
        {
            _monsterAI._canMove = false;
            _currentState = State.Attack;
        }
        else if (!IsPlayerInRange(_chaseRange))
        {
            _monsterAI._canMove = false;
            _currentState = State.Idle;
        }

    }

    private void Attack()
    {
        int randIndex = UnityEngine.Random.Range(0, 2);
        if (randIndex == 0)
        {
            _monsterSkill[0].OnSkill();
        }
        else
        {
            _monsterSkill[1].OnSkill();
        }

        if (_monsterData.MonsterHp <= 0)
        {
            _currentState = State.Die;   
        }
        else if (!IsPlayerInRange(_attackRange))
        {
            _currentState = State.Chase;
        }
        else if (!IsPlayerInRange(_chaseRange))
        {
            _currentState = State.Idle;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, _player.position) <= range;
    }
    
    public void TakeDamage(int value)
    {
        _monsterData.MonsterHp -= value;
        if (_monsterData.MonsterHp <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}
