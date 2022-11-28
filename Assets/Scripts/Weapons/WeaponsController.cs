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
    [SerializeField] private GameObject _pullTowardsTest;
    private bool playerIsPulling;
    public LayerMask layerMask;

    [Header("Bow&Arrow")]
    [SerializeField] GameObject gamePlayBow;
    [SerializeField] GameObject bowPosition;
    [SerializeField] GameObject bowPickablePrefab;
    private bool _isBowEquipped = true;
    private BowTest _bowTestRef;

    [SerializeField] private GameObject _weaponInRange;
    [SerializeField] private bool _weaponIsInRange = false;
    private Vector3 _weaponVelocityRef = Vector3.zero;

    private void Start()
    {
        //_bowTestRef = gamePlayBow.GetComponent<BowTest>();
    }

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
                playerIsPulling = true;
                // Testing between Vector3.Lerp & Vector3.SmoothDamp
                _weaponInRange.transform.position = Vector3.Slerp(
                _weaponInRange.transform.position, _pullTowardsTest.transform.position, 0.13f/*Time.deltaTime * _pullForce*/);
                //Vector3 targetPosition = _playerHands.TransformPoint(new Vector3(0, 0.5f, 0));
                Debug.Log("Pulling weapon");
                //_weaponInRange.GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(
                //GetComponent<Rigidbody>().velocity, _player.GetComponent<Rigidbody>().velocity, ref _weaponVelocityRef, Time.deltaTime * _pullForce);

            }
            else if (Input.GetMouseButtonUp(1) && playerIsPulling)
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
            if (_weaponInRange && _weaponIsInRange)
            {
                // FOR TESTING PURPOSES
                _isBowEquipped = true;
                //_bowTestRef.Reload();
                Destroy(collision.gameObject);
                Debug.Log("HI");
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
