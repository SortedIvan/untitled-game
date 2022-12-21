using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeThrown : MonoBehaviour
{

    [SerializeField] private float _throwForce = 20f;
    [SerializeField] private float _throwSpeed = 30f;

    private bool _hasToRotate;

    void Start()
    {
        //gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * (_throwForce * _throwSpeed));
        _hasToRotate = true;
    }

    void Update()
    {
        //gameObject.GetComponent<Rigidbody>().AddRelativeTorque(transform.right * _throwForce);

    }

    private void FixedUpdate()
    {
        if (_hasToRotate)
        {
            gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(Mathf.PI * 2, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _hasToRotate = false;
    }
}
