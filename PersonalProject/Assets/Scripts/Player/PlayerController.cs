using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;

    [SerializeField] private float _zoomMoveSpeed;
    [SerializeField] private float _normalMoveSpeed;

    // 공격 모드에 따른 이동 속도 필드
    private float _moveSpeed;
    private float _rotateSpeed = 10f;
    
    // 플레이어 오브젝트 회전을 위한 필드
    private Ray _rotateRay;
    public LayerMask _layerMask;
    private Camera _camera;
    
    // New InputActionSystem 입력을 위한 필드
    private NewInputAction _inputAction;
    private Vector2 _moveInput;
    private Vector3 _moveDir;
    private bool _isRightPressed;
    private bool _isLeftPressed;
    
    // Collider 충돌 확인을 위한 필드
    private SphereCollider _collider;
    
    private void Awake()
    {
        Init();
    }
    
    private void OnEnable()
    {
        _inputAction.PlayerAction.Enable();
        
        _inputAction.PlayerAction.Move.performed += OnMove;
        _inputAction.PlayerAction.Move.canceled += OnMove;
        _inputAction.PlayerAction.Zoom.performed += OnZoom;
        _inputAction.PlayerAction.Zoom.canceled += ZoomCancel;
        _inputAction.PlayerAction.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        _inputAction.PlayerAction.Move.performed -= OnMove;
        _inputAction.PlayerAction.Move.canceled -= OnMove;
        _inputAction.PlayerAction.Zoom.performed -= OnZoom;
        _inputAction.PlayerAction.Zoom.canceled -= ZoomCancel;
        _inputAction.PlayerAction.Attack.performed -= OnAttack;
        
        _inputAction.PlayerAction.Disable();
    }

    private void Update()
    {
        if (_isRightPressed)
        {
            RotateByMouse();
        }
        
        NormalMovement();

        if (_isLeftPressed)
        {
            Attack();
        }
    }

    private void Init()
    {
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
        _collider = GetComponentInChildren<SphereCollider>();
        _inputAction = new NewInputAction();
        _camera = Camera.main;
    }
    
    private void NormalMovement()
    {
        _moveDir = new Vector3(_moveInput.x, 0 , _moveInput.y);
        _moveSpeed = _isRightPressed ? _zoomMoveSpeed : _normalMoveSpeed;
        _characterController.Move(_moveDir * (_moveSpeed * Time.deltaTime));
        
        if (_moveDir == Vector3.zero || _isRightPressed) return;
        Quaternion targetRatate = Quaternion.LookRotation(_moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRatate, Time.deltaTime * _rotateSpeed);
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            
        }
    }

    private void Attack()
    {
        
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnZoom(InputAction.CallbackContext ctx)
    {
        _isRightPressed = true;
    }

    private void ZoomCancel(InputAction.CallbackContext ctx)
    {
        _isRightPressed = false;
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _isLeftPressed = true;
        }
    }

}
