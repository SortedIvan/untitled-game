using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Control")]
    public float movementSpeed;
    public float sprintMultiplier;

    [Header("Ground")]
    public LayerMask ground;
    public float groundDrag;
    public bool grounded;
    public float rayToGroundLength;

    [Header("Camera")]
    public Transform orientation;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    [Header("Keybinds")]
    public KeyCode jumpKey;

    private float _playerHeight;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private Rigidbody _playerRb;
    private bool _isPlayerSprint;
    private bool _isReadyToJump;

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerHeight = 1.8f;
        _isReadyToJump = true;
        jumpKey = KeyCode.Space;
        fallMultiplier = 2f;
        lowJumpMultiplier = 2f;
    }

    private void Update()
    {
        // Gather input
        GetPlayerInput();
        CheckPlayerSprinting();
        // Perform grounded check
        grounded = Physics.Raycast(transform.position, Vector3.down, rayToGroundLength, ground);
        ApplyFriction();
        SpeedControl();
        PlayerFalling();
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

        if (Input.GetKey(jumpKey) && _isReadyToJump && grounded)
        {
            Debug.Log("Jump");
            _isReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void CheckPlayerSprinting()
    {
        _isPlayerSprint = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift);
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (grounded)
        {
            if (_isPlayerSprint)
            {
                _playerRb.AddForce(_moveDirection.normalized * (movementSpeed + sprintMultiplier) * 10f, ForceMode.Force);
            }
            else if (!_isPlayerSprint)
            {
                _playerRb.AddForce(_moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
            }
        }
        else if (!grounded)
        {
            _playerRb.AddForce(_moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
        }

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

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_playerRb.velocity.x, 0f, _playerRb.velocity.z);
        if (_isPlayerSprint)
        {
            if (flatVel.magnitude > (movementSpeed + sprintMultiplier))
            {
                Vector3 limitedVel = flatVel.normalized * (movementSpeed + sprintMultiplier);
                _playerRb.velocity = new Vector3(limitedVel.x, _playerRb.velocity.y, limitedVel.z);
            }
        }
        else if (!_isPlayerSprint)
        {
            if (flatVel.magnitude > movementSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * movementSpeed;
                _playerRb.velocity = new Vector3(limitedVel.x, _playerRb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        // Making sure that the y velocity is 0, so we always jump the same height
        _playerRb.velocity = new Vector3(_playerRb.velocity.x, 0f, _playerRb.velocity.z);
        _playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void PlayerFalling()
    {
        if (_playerRb.velocity.y < 0)
        {
            _playerRb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }



    private void ResetJump()
    {
        _isReadyToJump = true;
    }
}