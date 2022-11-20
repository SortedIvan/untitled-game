using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update;
    
    [Header("Camera Options")]
    public float ySensitivity;
    public float xSensitivity;
    public GameObject playerOrientation;
    public float cameraDownConstraint;
    public float cameraUpConstraint;

    public Material testMaterial;
    public Material whiteMaterial;
    public float pickUpRange;
    public float pickUpRadius;


    private float _yRotation;
    private float _xRotation;
    private float _yInput;
    private float _xInput;

    /*
        Remarks collected from Unity forums ->
        Collect input in Update()
        Do movement in FixedUpdate() [especially if physics based]
        Move camera in LateUpdate() [more smooth]

        ---> Tested with LateUpdate, still lag
    */

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        // By default the player can't look up or down more than -+90 degrees
        cameraDownConstraint = 90;
        cameraUpConstraint = 90;
    }

    private void Update()
    {
        GetMouseInput();
        RotateCamera();

        ShootRay();
    }

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        //RotateCamera();
    }

    private void GetMouseInput()
    {
        _yInput = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * ySensitivity;
        _xInput = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * xSensitivity;
    }

    private void RotateCamera()
    {
        // Since Axis X & Y are switched in Unity
        _yRotation += _xInput;
        _xRotation -= _yInput; 
        // So that the player can't rotate beyond -90/90 degrees
        _xRotation = Mathf.Clamp(_xRotation, -cameraDownConstraint, cameraUpConstraint);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        playerOrientation.transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    private void ShootRay()
    {
        Transform cameraTransform = Camera.main.transform;
        RaycastHit HitInfo;
        if (Physics.SphereCast(cameraTransform.position, pickUpRadius,cameraTransform.forward, out HitInfo, pickUpRange))
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * pickUpRange, Color.yellow);
            if (HitInfo.transform.gameObject.name.Equals("Sword"))
            {
                HitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = testMaterial;
                Debug.Log(HitInfo.transform.gameObject.name);
            }
    
        }
    }

   

}
