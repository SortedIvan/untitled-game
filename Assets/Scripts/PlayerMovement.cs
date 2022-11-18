using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Control")]
    public float movementSpeed;
    public float sprintMultiplier;
    private bool _isPlayerSprint;

    [Header("Ground")]
    public LayerMask ground;
    public float groundDrag;
    public bool grounded;

    public Transform orientation;

    private float _playerHeight;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private Rigidbody _playerRb;

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerHeight = 1.8f;
    }

    private void Update()
    {
        // Gather input
        GetPlayerInput();
        CheckPlayerSprinting();
        // Perform grounded check
        grounded = Physics.Raycast(transform.position, Vector3.down, 2f, ground);
        ApplyFriction();
    }

    private void FixedUpdate()
    {
        // Movement (especially physics) should be done here
        MovePlayer();
    }

    private void GetPlayerInput()
    {
        this._horizontalInput = Input.GetAxisRaw("Horizontal");
        this._verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void CheckPlayerSprinting()
    {
        _isPlayerSprint = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift);
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        if (_isPlayerSprint)
        {
            _playerRb.AddForce(_moveDirection.normalized * (movementSpeed + sprintMultiplier) * 10f, ForceMode.Force);
        }
        _playerRb.AddForce(_moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
    }

    private void ApplyFriction()
    {
        if (grounded)
        {
            _playerRb.drag = groundDrag;
        }
        else if (!grounded)
        {
            _playerRb.drag = 0;
        }
    }

}