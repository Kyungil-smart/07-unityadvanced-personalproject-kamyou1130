using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Chase,
    Attack,
    Down,
    Die
}
public class MonsterController : MonoBehaviour, IDamagable
{
    [SerializeField] private int _monsterMaxHp;
    
    [SerializeField] private MonsterAI _monsterAI;
    [SerializeField] private Transform _player;
    [SerializeField] private List<MonsterSkill> _monsterSkill;
    
    // Monster 데이터 접근 필드 (MVP 패턴)
    private MonsterData _monsterData;
    
    // FSM 유한 상태 머신 접근 필드
    private State _currentState;
    public State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
        }
    }
    [SerializeField] private float _chaseRange;
    [SerializeField] private float _attackRange;
    
    // 시간 간격으로 공격 가능한지 확인하기 위한 필드
    private bool _isAttacking;
    public float _attackTime;

    // 자연스럽게 플레이어 오브젝트를 바라보도록 회전을 위한 시간 체크 필드
    private float _canMoveTime;
    
    // 애니메이션 접근 필드
    private Animator _animator;
    
    // 첫번째 몬스터 Die 상태 시 콜라이더 충돌을 방지하기 위한 필드
    private CapsuleCollider _capsuleCollider;
    
    // 두번째 몬스터 Die 상태 시 하위 오브젝트로부터 콜라이더를 얻기 위한 필드
    private CapsuleCollider _capsuleColliderInChildren;

    private bool _standing;
    private int cnt;
    private float _knowkDownTime;
    private Vector3 _originalPos;
    
    [SerializeField] private GameObject _flyObject;
    
    [SerializeField] private MonsterViewer _monsterViewer;

    [SerializeField] private Portal _portal;

    private PlayerController _playerController;

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
            case State.Down:
                Down();
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
        _monsterData.CurrentMonsterHp = _monsterMaxHp;
        _currentState = State.Idle;
        _capsuleCollider = GetComponent<CapsuleCollider>();
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player").transform;
        if (_flyObject != null) _originalPos = _flyObject.transform.position;
        if (_monsterViewer == null)  _monsterViewer = FindAnyObjectByType<MonsterViewer>();
        _monsterViewer.SetHpAmount(_monsterData.CurrentMonsterHp, _monsterMaxHp);
        if (_flyObject != null) _capsuleColliderInChildren = _flyObject.GetComponent<CapsuleCollider>();
        if (_playerController == null) _playerController = FindAnyObjectByType<PlayerController>();
    }

    private void Idle()
    {
        if (_playerController._isDead) return;
        
        if (IsPlayerInRange(_chaseRange))
        {
            _currentState = State.Chase;
        }
    }

    private void Chase()
    {
        _monsterAI._canMove = true;
        _animator.SetFloat("MoveSpeed", 1f);
        
        if (_monsterData.CurrentMonsterHp <= 0)
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

        if (_playerController._isDead)
        {
            _currentState = State.Idle;
        }
        
        if (_monsterData.CurrentMonsterHp <= 0)
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

    private void Down()
    {
        Debug.Log("넉다운");
        
        _knowkDownTime += Time.deltaTime;
        
        if (_flyObject.transform.position.y > _originalPos.y - 3.5f &&  _knowkDownTime < 3f)
        {
            _flyObject.transform.position += Vector3.down * (5f * Time.deltaTime);
        }

        if (_flyObject.transform.position.y < _originalPos.y && _knowkDownTime >= 3f)
        {
            _flyObject.transform.position += Vector3.up * (5f * Time.deltaTime);
            if (_flyObject.transform.position.y > _originalPos.y - 0.1f)
            {
                _standing = true;   
            }
        }
        
        if (_monsterData.CurrentMonsterHp <= 0)
        {
            _currentState = State.Die;
        }
        
        if (_standing)
        {
            if (IsPlayerInRange(_attackRange))
            {
                _currentState = State.Attack;
                _knowkDownTime = 0f;
                _standing = false;
            }
            else if (!IsPlayerInRange(_attackRange))
            {
                _currentState = State.Chase;
                _knowkDownTime = 0f;
                _standing = false;
            }
            else if (!IsPlayerInRange(_chaseRange))
            {
                _currentState = State.Idle;
                _knowkDownTime = 0f;
                _standing = false;
            }
        }
    }

    private void Die()
    {
        _animator.SetTrigger("Dead");
        if (_capsuleCollider != null)
        {
            _capsuleCollider.enabled = false;   
        }

        if (_capsuleColliderInChildren != null)
        {
            _capsuleColliderInChildren.enabled = false;
        }

        if (_portal == null) return;
        
        StartCoroutine(OpenPortalTime());
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
        _monsterData.CurrentMonsterHp -= value;
        
        _monsterViewer.SetHpAmount(_monsterData.CurrentMonsterHp, _monsterMaxHp);
    }

    private IEnumerator OpenPortalTime()
    {
        yield return new WaitForSeconds(3f);
        
        _portal.OpenPortal();
    }
    
    private IEnumerator Attacking()
    {
        if (_monsterSkill.Count <= 0) yield break;
        
        _isAttacking = true;
        
        _animator.SetTrigger("Attack");
        int randIndex = UnityEngine.Random.Range(0, _monsterSkill.Count);
        _animator.SetInteger("AttackIndex",randIndex);
        _monsterSkill[randIndex].OnSkill();
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
