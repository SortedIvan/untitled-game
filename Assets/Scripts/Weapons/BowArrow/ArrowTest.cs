using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTest : MonoBehaviour
{
    #region Variables
    [SerializeField] public float arrowSpeed = 20f;
    [SerializeField] public Transform arrowTip;
    [SerializeField] public Transform arrowShaft;

    private Rigidbody _arrowRb = null;
    private bool _arrowIsStopped = true;
    private Vector3 _lastArrowPosition = Vector3.zero;
    #endregion

    private void Awake()
    {
        _arrowRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_arrowIsStopped)
        {
            return;
        }

        // Rotate the arrow correctly and arching the arrow
        if (!_arrowRb.velocity.Equals(Vector3.zero))
        {
            _arrowRb.MoveRotation(Quaternion.LookRotation(_arrowRb.velocity, transform.up));
        }
        // Check for arrow collisions

        RaycastHit hit;
        if (!_lastArrowPosition.Equals(Vector3.zero))
        {
            if (Physics.Linecast(_lastArrowPosition, arrowTip.position, out hit))
            {
                Debug.Log(hit.collider.name);
                if (hit.rigidbody)
                {
                    transform.parent = hit.collider.gameObject.transform;
                    hit.rigidbody.AddForce(_arrowRb.velocity * 5f, ForceMode.Force);
                }
                StopArrow();

                // IF NO RIGIDBODY

            }
        }
        // Store arrow position;
        _lastArrowPosition = arrowTip.position;

    }

    private void StopArrow()
    {
        _arrowIsStopped = true;
        _arrowRb.isKinematic = true;
        _arrowRb.useGravity = false;
    }

    public void FireArrow(float pullStrength)
    {
        _arrowIsStopped = false;
        transform.parent = null;

        _arrowRb.isKinematic = false;
        _arrowRb.useGravity = true;
        _arrowRb.AddForce(transform.forward * (pullStrength * arrowSpeed));

        Destroy(gameObject, 5f);
    }
}
