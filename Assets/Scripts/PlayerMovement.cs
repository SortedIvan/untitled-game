using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Control")]
    public float movementSpeed;

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
        // Perform grounded check
        grounded = Physics.Raycast(transform.position, Vector3.down, 2f, ground);
    }

    private void FixedUpdate()
    {
        // Movement (especially physics) should be done here
        MovePlayer();
        ApplyFriction();
    }

    private void GetPlayerInput()
    {
        this._horizontalInput = Input.GetAxisRaw("Horizontal");
        this._verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Since it is a camera based input
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        _playerRb.AddForce(_moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
    }

    private void ApplyFriction()
    {
        if (grounded)
        {
            Vector3 currentVelocity = _playerRb.velocity;
            _playerRb.velocity = currentVelocity * 0.85f;
        }
    }
}