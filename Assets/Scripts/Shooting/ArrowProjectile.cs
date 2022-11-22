using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private float _arrowDamage;
    [SerializeField] private float _arrowTorque;
    [SerializeField] private Rigidbody _arrowRb;
    [SerializeField] private float _firePower;

    private string _enemyTag;
    private bool _didHit;
    private GameObject _anchor;

    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        Fire();
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        if (!_didHit)
        {
            transform.rotation = Quaternion.LookRotation(_arrowRb.velocity);
        }
        else if (_didHit){
            this.transform.position = _anchor.transform.position;
            this.transform.rotation = _anchor.transform.rotation;
        }
    }

    private void Fire()
    {
        //var force = Camera.main.transform.TransformDirection(Vector3.forward * firePower);
        var force = Camera.main.transform.TransformDirection(Vector3.forward * _firePower);
        Fly(force);
    }

    private void Fly(Vector3 force)
    {
        _arrowRb.isKinematic = false;
        _arrowRb.AddForce(force, ForceMode.Impulse);
        _arrowRb.AddTorque(transform.right * _arrowTorque);
        transform.SetParent(null);
    }

    public void SetForce(float force)
    {
        this._firePower = force;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _didHit = true;
        this.transform.position = collision.contacts[0].point;
        this.transform.GetComponent<BoxCollider>().isTrigger = true;
        GameObject arrowAnchor = new GameObject("ARROW_ANCHOR");
        arrowAnchor.transform.position = this.transform.position;
        arrowAnchor.transform.rotation = this.transform.rotation;
        arrowAnchor.transform.parent = collision.transform;
        _anchor = arrowAnchor;
        Destroy(_arrowRb);
        //transform.SetParent(collision.transform);
    }

}
