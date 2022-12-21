using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeThrown : MonoBehaviour
{

    [SerializeField] private float _throwForce = 20f;
    [SerializeField] private float _throwSpeed = 30f;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * (_throwForce * _throwSpeed));
    }

    void Update()
    {
        gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.forward);
    }
}
