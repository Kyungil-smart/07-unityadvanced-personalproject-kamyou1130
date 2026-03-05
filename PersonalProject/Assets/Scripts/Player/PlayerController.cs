using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class PlayerController : MonoBehaviour, IDamagable
{
    // CharacterController 컴포넌트 정보를 가져오기 위한 필드
    [SerializeField] private CharacterController _characterController;

    // 이동 속도를 설정하기 위한 필드
    [SerializeField] private float _zoomMoveSpeed;
    [SerializeField] private float _normalMoveSpeed;
    
    // 플레이어 데이터 접근 필드(MVP 패턴)
    private PlayerData _playerData;

    // 플레이어 공격 모드에 따른 이동 속도 필드
    public float _moveSpeed;
    private float _rotateSpeed = 10f;
    
    // 플레이어 공격 접근 필드
    private float _attackableTime;
    [SerializeField] private Transform _attackPos;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _attackLayer;
    
    // 플레이어 대쉬 접근 필드
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashCoolTime;
    
    // 플레이어 오브젝트 회전을 위한 필드
    private Ray _rotateRay;
    [SerializeField] private LayerMask _layerMask;
    public Camera _camera;
    
    // New InputActionSystem 입력을 위한 필드
    private NewInputAction _inputAction;
    private Vector2 _moveInput;
    private Vector3 _moveDir;
    private bool _isRightPressed;
    private bool _isLeftPressed;
    public bool _isParryingPressed;
    private bool _dashPressed;
    
    // 플레이어 애니메이션 접근 필드
    private Animator _animator;
    
    // 콜라이더 상호작용을 위한 필드
    private MonsterController _monster;
    private SphereCollider _sphereCollider;
    
    // 폭탄 설치 코루틴 접근 필드
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private float _bombCoolTime;

    // MVP 패턴을 위한 클래스 선언 필드
    private PlayerViewer _playerViewer;
    [SerializeField] private int _playerMaxHp;
    private SkillIconViewer _skillIconViewer;
    
    // 중력을 받기 위한 필드
    private Vector3 _velocity;

    public bool _isLock = true;

    public bool _isDead;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    
    private void OnEnable()
    {
        _inputAction.PlayerAction.Enable();
        
        _inputAction.PlayerAction.Move.performed += OnMove;
        _inputAction.PlayerAction.Move.canceled += MoveCancel;
        _inputAction.PlayerAction.Zoom.performed += OnZoom;
        _inputAction.PlayerAction.Zoom.canceled += ZoomCancel;
        _inputAction.PlayerAction.Attack.performed += OnAttack;
        _inputAction.PlayerAction.Dash.performed += OnDash;
        _inputAction.PlayerAction.Parrying.performed += OnParrying;
        _inputAction.PlayerAction.Bomb.performed += OnBomb;
    }

    private void OnDisable()
    {
        _inputAction.PlayerAction.Move.performed -= OnMove;
        _inputAction.PlayerAction.Move.canceled -= MoveCancel;
        _inputAction.PlayerAction.Zoom.performed -= OnZoom;
        _inputAction.PlayerAction.Zoom.canceled -= ZoomCancel;
        _inputAction.PlayerAction.Attack.performed -= OnAttack;
        _inputAction.PlayerAction.Dash.performed -= OnDash;
        _inputAction.PlayerAction.Parrying.performed -= OnParrying;
        _inputAction.PlayerAction.Bomb.performed -= OnBomb;
        
        _inputAction.PlayerAction.Disable();
    }

    private void Update()
    {
        if (_isDead) return;
        
        _playerData.CurrentBombCooltime += Time.deltaTime;
        _skillIconViewer.SetBomb(_playerData.CurrentBombCooltime, _bombCoolTime);
        
        if (_isRightPressed)
        {
            RotateByMouse();
            if (_isLeftPressed)
            {
                TryAttack();
            }
        }
        
        Gravity();
        
        _attackableTime += Time.deltaTime;
        _skillIconViewer.SetAttack(_attackableTime, 0.5f);
        if (_attackableTime >= 0.5f)
        {
            Movement();
        }
        
        _playerData.CurrentDashCooltime += Time.deltaTime;
        _skillIconViewer.SetDash(_playerData.CurrentDashCooltime, _dashCoolTime);
        if (_dashPressed)
        {
            _dashPressed = false;
            Dash();
        }

    }

    private void Init()
    {
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
        if (_sphereCollider == null) _sphereCollider = GetComponent<SphereCollider>();
        _inputAction = new NewInputAction();
        _playerData = new PlayerData();
        _playerData.CurrentPlayerHp = _playerMaxHp;
        _camera = Camera.main;
        _dashPressed = false;
        _animator = GetComponentInChildren<Animator>();
        _playerData.CurrentDashCooltime = _dashCoolTime;
        _playerData.CurrentBombCooltime = _bombCoolTime;
        _attackableTime = 0.5f;
        if (_playerViewer == null) _playerViewer = FindAnyObjectByType<PlayerViewer>();
        _playerViewer.SetPlayerHp(_playerData.CurrentPlayerHp, _playerMaxHp);
        if (_skillIconViewer == null) _skillIconViewer = FindAnyObjectByType<SkillIconViewer>();
    }
    
    private void Movement()
    {
        _moveDir = new Vector3(_moveInput.x, 0 , _moveInput.y);
        _moveSpeed = _isRightPressed ? _zoomMoveSpeed : _normalMoveSpeed;
        _characterController.Move(_moveDir * (_moveSpeed * Time.deltaTime));
        
        if (_moveDir == Vector3.zero || _isRightPressed) return;
        Quaternion targetRotate = Quaternion.LookRotation(_moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotate, Time.deltaTime * _rotateSpeed);
    }

    private void RotateByMouse()
    {
        _rotateRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(_rotateRay, out hit, 100f, _layerMask))
        {
            Vector3 targetPoint = hit.point;
            
            targetPoint.y = _characterController.transform.position.y;
            
            transform.LookAt(targetPoint);
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (_attackableTime > 0f) return;
        if (other.gameObject.CompareTag("Monster") && _isLeftPressed && _isRightPressed)
        {
            _monster = other.gameObject.GetComponent<MonsterController>();
            StartCoroutine(AttackTiming());
            Debug.Log("공격!");
            
            _attackableTime = 0.5f;
            _isLeftPressed = false;
        }
    }
    */
    
    private void TryAttack()
    {
        if (_attackableTime < 0.5f) return;
        
        _animator.SetTrigger("Attack");
        int randIndex = UnityEngine.Random.Range(0, 3);
        _animator.SetInteger("AttackIndex", randIndex);
        AudioManager.Instance.PlaySound(Resources.Load<AudioClip>("Audio/Slash"));
        
        Collider[] colliders = Physics.OverlapSphere(_attackPos.position, _attackRange, _attackLayer);

        foreach (Collider hit in colliders)
        {
            _monster = hit.GetComponentInParent<MonsterController>();
            StartCoroutine(AttackTiming());
            
            Debug.Log("공격!");
        }

        _attackableTime = 0f;
    }

    private void Dash()
    {
        if (_playerData.CurrentDashCooltime < _dashCoolTime || _isRightPressed) return;
        _playerData.CurrentDashCooltime = 0f;
        StartCoroutine(DashCoroutine());
    }
    
    private void BombSet()
    {
        if (_isLock) return;
        if (_playerData.CurrentBombCooltime < _bombCoolTime) return;
        if (_bombPrefab == null) return;
        
        Instantiate(_bombPrefab, transform.position, Quaternion.identity);

        _playerData.CurrentBombCooltime = 0f;
    }

    private void Gravity()
    {
        _velocity.y += Physics.gravity.y * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private bool IsGround()
    {
        Vector3 ray = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(ray, Vector3.down, 0.1f, _layerMask))
        {
            return true;
        }
        
        return false;
    }
    
    // 마우스 좌클릭을 눌렀을때 0.05초 후에 false로 바꾸기 위한 코루틴
    private IEnumerator AttackableCount()
    {
        yield return new WaitForSeconds(0.05f);

        _isLeftPressed = false;
    }

    private IEnumerator DashCoroutine()
    {
        float timer = Time.time;
        Vector3 moveDir = new Vector3(_moveInput.x, 0 , _moveInput.y);
        while (timer + 0.2f > Time.time)
        {
            _characterController.Move(moveDir * (_dashSpeed * Time.deltaTime));
            yield return null;
        }
    }

    // 공격기능과 애니메이션 시간을 맞추기위한 코루틴
    private IEnumerator AttackTiming()
    {
        yield return new WaitForSeconds(0.08f);
        _monster.TakeDamage(_playerData.PlayerAttackDamage);
    }

    private IEnumerator ParryingCoroutine()
    {
        if (!_isRightPressed) yield break;
        _animator.SetBool("Parry", true);
        
        yield return new WaitForSeconds(0.2f);
        
        _animator.SetBool("Parry", false);
        _isParryingPressed = false;
    }

    public void TakeDamage(int value)
    {
        _playerData.CurrentPlayerHp -= value;
        Debug.Log($"{value} 데미지를 입음, 남은 체력 {_playerData.CurrentPlayerHp}");
        
        _playerViewer.SetPlayerHp(_playerData.CurrentPlayerHp, _playerMaxHp);

        if (_playerData.CurrentPlayerHp <= 0)
        {
            Die();
        }
    }

    public void UnLocking()
    {
        _skillIconViewer.UnLock();
    }

    private void Die()
    {
        _animator.SetTrigger("Dead");
        
        _isDead = true;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
        _animator.SetFloat("MoveSpeed", Mathf.Abs(1f));
    }

    private void MoveCancel(InputAction.CallbackContext ctx)
    {
        _moveInput = Vector2.zero;
        _animator.SetFloat("MoveSpeed", Mathf.Abs(0f));
    }

    private void OnZoom(InputAction.CallbackContext ctx)
    {
        _isRightPressed = true;
        _animator.SetBool("Zoom", true);
    }

    private void ZoomCancel(InputAction.CallbackContext ctx)
    {
        _isRightPressed = false;
        _animator.SetBool("Zoom", false);
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        _isLeftPressed = true;

        StartCoroutine(AttackableCount());
    }

    private void OnDash(InputAction.CallbackContext ctx)
    {
        _dashPressed = true;
    }

    private void OnParrying(InputAction.CallbackContext ctx)
    {
        _isParryingPressed = true;
        
        StartCoroutine(ParryingCoroutine());
    }

    private void OnBomb(InputAction.CallbackContext ctx)
    {
        BombSet();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
        Gizmos.DrawSphere(_attackPos.position, _attackRange);
    }
}
