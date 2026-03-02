using System.Collections;
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
    [SerializeField] private float _chaseRange;
    [SerializeField] private float _attackRange;
    
    // 시간 간격으로 공격 가능한지 확인하기 위한 필드
    private bool _isAttacking;
    public float _attackTime;

    // 자연스럽게 플레이어 오브젝트를 바라보도록 회전을 위한 시간 체크 필드
    private float _canMoveTime;
    
    // 애니메이션
    private Animator _animator;
    
    private CapsuleCollider _capsuleCollider;

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
                Die();
                break;
        }
    }

    private void Init()
    {
        if (_monsterAI == null) _monsterAI = FindAnyObjectByType<MonsterAI>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
        _monsterData = new MonsterData();
        _currentState = State.Idle;
        _capsuleCollider = GetComponent<CapsuleCollider>();
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
        _animator.SetFloat("MoveSpeed", 1f);
        
        if (_monsterData.MonsterHp <= 0)
        {
            _monsterAI._canMove = false;
            _currentState = State.Die;
            _animator.SetFloat("MoveSpeed", 0f);
        }
        else if (IsPlayerInRange(_attackRange))
        {
            _monsterAI._canMove = false;
            _currentState = State.Attack;
            _animator.SetFloat("MoveSpeed", 0f);
        }
        else if (!IsPlayerInRange(_chaseRange))
        {
            _monsterAI._canMove = false;
            _currentState = State.Idle;
            _animator.SetFloat("MoveSpeed", 0f);
        }
    }

    private void Attack()
    {
        if (!_isAttacking)
        {
            StartCoroutine(Attacking());
        }

        _canMoveTime += Time.deltaTime;
        if (_canMoveTime > 2f)
        {
            RotateToPlayer();
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

    private void Die()
    {
        _animator.SetTrigger("Dead");
        _capsuleCollider.enabled = false;
    }

    private void RotateToPlayer()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, _player.position) <= range;
    }
    
    public void TakeDamage(int value)
    {
        _monsterData.MonsterHp -= value;
    }
    
    private IEnumerator Attacking()
    {
        _isAttacking = true;
        
        _animator.SetTrigger("Attack");
        int RandIndex = UnityEngine.Random.Range(0, _monsterSkill.Count);
        _animator.SetInteger("AttackIndex",RandIndex);
        _monsterSkill[RandIndex].OnSkill();
        _canMoveTime = 0f;
        
        yield return new WaitForSeconds(_attackTime);
        
        _isAttacking = false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,0,1, 0.3f);
        Gizmos.DrawSphere(transform.position, _chaseRange);
        
        Gizmos.color = new Color(1,0,0, 0.3f);
        Gizmos.DrawSphere(transform.position, _attackRange);
    }
}
