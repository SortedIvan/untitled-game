using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{

    #region Variables
    [Header("Player Logic")]
    [SerializeField] private GameObject _player;
    [SerializeField] float throwForceWeapons;
    [SerializeField] private Transform _playerHands;
    [SerializeField] private GameObject _pullTowardsTest;
    [Range(0.1f, 1f)] public float sphereCastRadius;
    [Range(1f, 100f)] public float range;


    [Header("Main weapon logic")]
    [SerializeField] private GameObject _weaponInRange;
    [SerializeField] private bool _weaponIsInRange = false;
    private bool _weaponIsEquipped = false;


    [Header("Bow&Arrow")]
    [SerializeField] GameObject bowPosition;
    [SerializeField] GameObject bowPickablePrefab;
    private bool _bowIsEqupped;

    [Header("Shotgun")]
    [SerializeField] GameObject shotgunPosition;
    [SerializeField] GameObject shotgunPickablePrefab;
    private bool _shotgunIsEqupped;

    [Header("Axe")]
    [SerializeField] private GameObject _axePosition;
    [SerializeField] private GameObject _axePickablePrefab;
    [SerializeField] private float _axeThrowPower;
    private bool _axeIsEqupped;
    [SerializeField] private float _throwForce = 20f;
    [SerializeField] private float _throwSpeed = 30f;

    [Header("Pulling")]
    [SerializeField] private GameObject _electricEffect;
    [SerializeField] private Transform _electricEndPos;
    [SerializeField] private Transform _electricMidPoint;
    [SerializeField] private Transform _electricMidPointTwo;
    [SerializeField] private float _pullForce;
    [SerializeField] private float _maxRangeWeaponPickup;
    [SerializeField] private float _pullVelocityY = 4.7f;
    private bool _playerIsPulling;
    #endregion

    private void Start()
    {
    }

    private void Update()
    {
        ThrowWeapon();
        ThrowAxe();
        DetectWeapon();
        ResetWeaponInRange();
        if (Input.GetMouseButton(1))
        {
            if (_weaponInRange && _weaponIsInRange)
            {
                _playerIsPulling = true;

            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            _playerIsPulling = false;
            _electricEffect.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        float playerSpeed = _player.GetComponent<Rigidbody>().velocity.magnitude;
        float pullSpeed = 0.11f;
        if (playerSpeed > 9.5f)
        {
            pullSpeed = 0.3f;
        }

        if (_playerIsPulling && !_weaponIsEquipped)
        {

            _electricEffect.SetActive(true);

            // SLERP ----------------------------------------------------------
            Vector3 slerp = Vector3.Lerp(
            _weaponInRange.transform.position,
            _pullTowardsTest.transform.position, pullSpeed
            );

            // Test velocity null
            _weaponInRange.GetComponent<Rigidbody>().velocity = Vector3.zero;



            if (!(slerp.y >= _pullVelocityY))
            {
                slerp.y += 0.1f;
                Debug.Log(slerp.y);
            }


            _weaponInRange.transform.position = slerp;


            // Check if the point is very close
            if (Mathf.Abs(_pullTowardsTest.transform.position.x - _weaponInRange.transform.position.x) < 0.05)
            {
                _weaponInRange.transform.position = _pullTowardsTest.transform.position;
            }

            Debug.Log("HI");
            float distance = Vector3.Distance(_weaponInRange.transform.position, _pullTowardsTest.transform.position);


            // A
            _electricMidPoint.position = slerp;
            _electricMidPointTwo.position = slerp;
            _electricEndPos.position = slerp;

            Quaternion cameraRotation = Quaternion.LookRotation(Camera.main.transform.forward);
            _weaponInRange.transform.rotation = Quaternion.Lerp(_weaponInRange.transform.rotation, cameraRotation, 0.13f);
            Debug.Log("Pulling weapon");

            if (distance < 1f)
            {
                _electricEffect.SetActive(false);
                CheckIfWeaponEquipped(_weaponInRange.gameObject.name);
                _playerIsPulling = false;
                Destroy(_weaponInRange);
            }
        }
    }

    private void CheckIfWeaponEquipped(string weapon)
    {

        if (weapon.Contains("BowPickable"))
        {
            bowPosition.SetActive(true);
            _bowIsEqupped = true;
            _weaponIsEquipped = true;
        }

        if (weapon.Contains("ShotgunPickable"))
        {
            shotgunPosition.SetActive(true);
            _shotgunIsEqupped = true;
            _weaponIsEquipped = true;
        }

        if (weapon.Contains("AxePickable"))
        {
            _axePosition.SetActive(true);
            _axeIsEqupped = true;
            _weaponIsEquipped = true;
        }
       
    }

    private void ThrowWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_bowIsEqupped)
            {
                bowPosition.SetActive(false);
                _bowIsEqupped = false;
                _weaponIsEquipped = false;

                bowPickablePrefab.transform.rotation = bowPosition.transform.rotation;
                bowPickablePrefab.transform.position = bowPosition.transform.position;
                GameObject bowThrowed = Instantiate(bowPickablePrefab);
                bowThrowed.transform.parent = null;
                bowThrowed.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwForceWeapons;
            }
            if (_shotgunIsEqupped)
            {
                shotgunPosition.SetActive(false);
                _shotgunIsEqupped = false;
                _weaponIsEquipped = false;

                shotgunPickablePrefab.transform.rotation = shotgunPosition.transform.rotation;
                shotgunPickablePrefab.transform.position = shotgunPosition.transform.position;
                GameObject shotgunThrowed = Instantiate(shotgunPickablePrefab);
                shotgunThrowed.transform.parent = null;
                shotgunThrowed.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwForceWeapons;
            }
            if (_axeIsEqupped)
            {
                _axePosition.SetActive(false);
                _axeIsEqupped = false;
                _weaponIsEquipped = false;

                _axePickablePrefab.transform.rotation = _axePosition.transform.rotation;
                _axePickablePrefab.transform.position = _axePosition.transform.position;
                GameObject axeThrown = Instantiate(_axePickablePrefab);
                axeThrown.transform.parent = null;
                axeThrown.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwForceWeapons;
            }
        }
    }

    private void ThrowAxe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_axeIsEqupped)
            {
                _axePosition.SetActive(false);
                _axeIsEqupped = false;
                _weaponIsEquipped = false;

                _axePickablePrefab.transform.rotation = _axePosition.transform.rotation;
                _axePickablePrefab.transform.position = _axePosition.transform.position;
                GameObject axeThrown = Instantiate(_axePickablePrefab);
                axeThrown.transform.parent = null;
                axeThrown.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * (_throwForce * _throwSpeed));

                // TODO:CHANGE
                _weaponInRange = axeThrown;
            }
        }
    }

    private void ResetWeaponInRange()
    {
        if (_weaponInRange)
        {
            float weaponAwayFromPlayerDist = Vector3.Distance(_player.transform.position, _weaponInRange.transform.position);
            if (weaponAwayFromPlayerDist >= _maxRangeWeaponPickup)
            {
                _weaponIsInRange = false;
                _weaponInRange = null;
            }
        }
    }

    private void DetectWeapon()
    {
        RaycastHit hit;
        Vector3 pullHandsPosition = new Vector3(_playerHands.position.x, _playerHands.position.y + 0.2f, _playerHands.position.z);
        if (Physics.SphereCast(pullHandsPosition, sphereCastRadius, Camera.main.transform.forward * range, out hit, range))
        {
            if (hit.transform.tag.Equals("Weapon"))
            {
                _weaponIsInRange = true;
                _weaponInRange = hit.transform.gameObject;

                return;
            }

        }
    }

    private void PullWeapon()
    {
        // Checking if weapon is in range and the weapon object is not null
        if (_weaponInRange && _weaponIsInRange)
        {
            if (Input.GetMouseButton(1))
            {
                _playerIsPulling = true;
                // Testing between Vector3.Lerp & Vector3.SmoothDamp
                _weaponInRange.transform.position = Vector3.Slerp(
                _weaponInRange.transform.position,
                _pullTowardsTest.transform.position, 0.13f
                /*Time.deltaTime * _pullForce*/);

                Quaternion cameraRotation = Quaternion.LookRotation(Camera.main.transform.forward);
                _weaponInRange.transform.rotation = Quaternion.Lerp(_weaponInRange.transform.rotation, cameraRotation, 0.13f);
                Debug.Log("Pulling weapon");
            }
            else if (Input.GetMouseButtonUp(1))
            {
                _playerIsPulling = false;
            }
        }
    }

    // TODO: Move this onto another part (maybe hands)
    private void OnCollisionEnter(Collision collision)
    {
        if (_playerIsPulling && collision.gameObject.tag.Equals("Weapon"))
        {
            if (!_weaponIsEquipped)
            {
                if (_weaponInRange && _weaponIsInRange)
                {
                    // FOR TESTING PURPOSES
                    _electricEffect.SetActive(false);
                    CheckIfWeaponEquipped(collision.gameObject.name);
                    _playerIsPulling = false;
                    Destroy(collision.gameObject);
                    Debug.Log("HI");
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_playerHands.position, range);
        Vector3 pullHandsPosition = new Vector3(_playerHands.position.x, _playerHands.position.y + 0.2f, _playerHands.position.z);
        RaycastHit hit;
        if (Physics.SphereCast(pullHandsPosition, sphereCastRadius, Camera.main.transform.forward * range, out hit, range))
        {
            Gizmos.color = Color.green;
            Vector3 sphereCastMidpoint = pullHandsPosition + (Camera.main.transform.forward * hit.distance);
            Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
            Gizmos.DrawSphere(hit.point, 0.1f);
            Debug.DrawLine(pullHandsPosition, sphereCastMidpoint, Color.green);
        }
        else
        {
            Gizmos.color = Color.red;
            Vector3 sphereCastMidpoint = _playerHands.position + (Camera.main.transform.forward * (range - sphereCastRadius));
            Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
            Debug.DrawLine(transform.position, sphereCastMidpoint, Color.red);
        }
    }



}
