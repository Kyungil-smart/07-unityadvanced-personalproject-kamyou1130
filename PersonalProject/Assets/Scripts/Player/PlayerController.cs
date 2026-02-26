using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;

    [SerializeField] private float _moveSpeed;

    private Ray _rotateRay;
    public LayerMask _layerMask;
    private Camera _camera;
    
    private NewInputAction _inputAction;
    private Vector2 _moveInput;

    private void Awake()
    {
        Init();
    }
    
    private void OnEnable()
    {
        _inputAction.PlayerAction.Enable();
        
        _inputAction.PlayerAction.Move.performed += OnMove;
        _inputAction.PlayerAction.Move.canceled += MoveCancel;
    }

    private void OnDisable()
    {
        _inputAction.PlayerAction.Move.performed -= OnMove;
        _inputAction.PlayerAction.Move.canceled -= MoveCancel;
        
        _inputAction.PlayerAction.Disable();
    }

    private void Update()
    {
        Movement();
        RotateByMouse();
    }

    private void Init()
    {
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
        _inputAction = new NewInputAction();
        _camera = Camera.main;
    }

    private void Movement()
    {
        Vector3 moveDir = transform.forward * _moveInput.y + transform.right * _moveInput.x;
        _characterController.Move(moveDir * (_moveSpeed * Time.deltaTime));
    }

    private void RotateByMouse()
    {
        _rotateRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(_rotateRay, out hit, 100f, _layerMask))
        {
            Vector3 targetPoint = hit.point;
            
            targetPoint.y = _characterController.transform.position.y;
            
            transform.LookAt(hit.point);
        }
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    private void MoveCancel(InputAction.CallbackContext ctx)
    {
        _moveInput = Vector2.zero;
    }

}
