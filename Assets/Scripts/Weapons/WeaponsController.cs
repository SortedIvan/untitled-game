using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [Header("Player Logic")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _playerOrientation;
    [SerializeField] float throwForceWeapons;
    [SerializeField] private Transform _playerHands;
    [Range(0.1f, 1f)] public float sphereCastRadius;
    [Range(1f, 100f)] public float range;
    [SerializeField] private float _pullForce;
    [SerializeField] private float _maxRangeWeaponPickup;
    public LayerMask layerMask;


    [Header("Bow&Arrow")]
    [SerializeField] GameObject gamePlayBow;
    [SerializeField] GameObject bowPosition;
    [SerializeField] GameObject bowPickablePrefab;
    private bool _isBowEquipped = true;


    private GameObject _weaponInRange;
    private bool _weaponIsInRange = false;

    private void Update()
    {
        CheckIfBowEquipped();
        ThrowBow();
        DetectWeapon();
        ResetWeaponInRange();
    }

    private void FixedUpdate()
    {
        PullWeapon();
    }

    private void CheckIfBowEquipped()
    {
        bowPosition.SetActive(_isBowEquipped); 
    }

    private void ThrowBow()
    {
        if (_isBowEquipped && Input.GetKeyDown(KeyCode.Q))
        {
            bowPickablePrefab.transform.rotation = bowPosition.transform.rotation;
            bowPickablePrefab.transform.position = bowPosition.transform.position;
            GameObject bowThrowed = Instantiate(bowPickablePrefab);
            _isBowEquipped = false;
            bowThrowed.transform.parent = null;
            //bowThrowed.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwForceWeapons, ForceMode.Impulse);
            Debug.Log(bowThrowed.transform.position);
            Debug.Log(Camera.main.transform.forward);
            bowThrowed.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwForceWeapons;
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
        if (Physics.SphereCast(_playerHands.position, sphereCastRadius, Camera.main.transform.forward * range, out hit, range))
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
                _weaponInRange.transform.position = Vector3.Lerp(
                _weaponInRange.transform.position, _playerHands.position, Time.deltaTime * _pullForce);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
        RaycastHit hit;
        if (Physics.SphereCast(_playerHands.position, sphereCastRadius, Camera.main.transform.forward * range, out hit, range))
        {
            Gizmos.color = Color.green;
            Vector3 sphereCastMidpoint = _playerHands.position + (Camera.main.transform.forward * hit.distance);
            Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
            Gizmos.DrawSphere(hit.point, 0.1f);
            Debug.DrawLine(transform.position, sphereCastMidpoint, Color.green);
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
