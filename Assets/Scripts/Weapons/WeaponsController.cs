using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [Header("Player Logic")]
    [SerializeField] private GameObject _player;
    [SerializeField] float throwForceWeapons;
    [SerializeField] private Transform _playerHands;
    [SerializeField] private float _pullForce;
    [SerializeField] private float _maxRangeWeaponPickup;
    [SerializeField] private GameObject _pullTowardsTest;
    [Range(0.1f, 1f)] public float sphereCastRadius;
    [Range(1f, 100f)] public float range;
    private bool playerIsPulling;

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

    [SerializeField] private GameObject _electricEffect;
    [SerializeField] private Transform _electricEndPos;
    [SerializeField] private Transform _electricMidPoint;
    [SerializeField] private Transform _electricMidPointTwo;



    private void Start()
    {
    }

    private void Update()
    {
        ThrowWeapon();
        DetectWeapon();
        ResetWeaponInRange();
        if (Input.GetMouseButton(1))
        {
            if (_weaponInRange && _weaponIsInRange)
            {
                playerIsPulling = true;

            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            playerIsPulling = false;
            _electricEffect.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        float playerSpeed = _player.GetComponent<Rigidbody>().velocity.magnitude;
        float pullSpeed = 0.17f;
        if (playerSpeed > 9.5f)
        {
            pullSpeed = 0.21f;
        }

        if (playerIsPulling && !_weaponIsEquipped)
        {

            _electricEffect.SetActive(true);

            // SLERP ----------------------------------------------------------
            Vector3 slerp = Vector3.Lerp(
            _weaponInRange.transform.position,
            _pullTowardsTest.transform.position, pullSpeed
            );
            // Testing between Vector3.Lerp & Vector3.SmoothDamp
            _electricEndPos.position = slerp;
            _weaponInRange.transform.position = slerp;

            _electricMidPoint.position = slerp;
            _electricMidPointTwo.position = slerp;

            Quaternion cameraRotation = Quaternion.LookRotation(Camera.main.transform.forward);
            _weaponInRange.transform.rotation = Quaternion.Lerp(_weaponInRange.transform.rotation, cameraRotation, 0.13f);
            Debug.Log("Pulling weapon");
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
                Debug.Log(bowThrowed.transform.position);
                Debug.Log(Camera.main.transform.forward);
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
                Debug.Log(shotgunThrowed.transform.position);
                Debug.Log(Camera.main.transform.forward);
                shotgunThrowed.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwForceWeapons;
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
                playerIsPulling = true;
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
                playerIsPulling = false;
            }
        }
    }

    // TODO: Move this onto another part (maybe hands)
    private void OnCollisionEnter(Collision collision)
    {
        if (playerIsPulling && collision.gameObject.tag.Equals("Weapon"))
        {
            if (!_weaponIsEquipped)
            {
                if (_weaponInRange && _weaponIsInRange)
                {
                    // FOR TESTING PURPOSES
                    _electricEffect.SetActive(false);
                    CheckIfWeaponEquipped(collision.gameObject.name);
                    playerIsPulling = false;
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
