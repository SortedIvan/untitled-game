using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowShooting : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private GameObject _orientation;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _forceMultiplier;
    [SerializeField] private GameObject _bow;
    private GameObject _currentArrow;
    private float _currentArrowForce;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        _bow.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        //if (!_currentArrow.Equals(null))
        //{

        //}
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            //_currentArrow = Instantiate(_arrowPrefab, _shootPoint.position, Quaternion.LookRotation(_orientation.transform.forward));
            _currentArrow = Instantiate(_arrowPrefab, _shootPoint.position, Quaternion.LookRotation(Camera.main.transform.forward));
            
            _currentArrow.GetComponent<Rigidbody>().isKinematic = true;
            _currentArrow.transform.SetParent(_shootPoint);
        }
        if (Input.GetMouseButton(0))
        {
            if (_currentArrowForce >= _maxForce)
            {
                _currentArrowForce = _maxForce;
            }
            else if (!(_currentArrowForce >= _maxForce))
            {
                _currentArrowForce += Time.deltaTime * 5f * _forceMultiplier;
                Debug.Log($"Force is: {_currentArrowForce}");
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Quaternion newRotation; /*= Quaternion.Euler(Camera.main.transform.forward);*/
            //newRotation = Quaternion.LookRotation(Camera.main.transform.forward);
            //_currentArrow = Instantiate(_arrowPrefab, _shootPoint.position, newRotation);
            //_currentArrow = Instantiate(_arrowPrefab);
            _currentArrow.GetComponent<Rigidbody>().isKinematic = false;
            _currentArrow.transform.SetParent(null);
            _currentArrow.GetComponent<ArrowProjectile>().SetForce(_currentArrowForce);
            _currentArrow.GetComponent<ArrowProjectile>().ShootArrow();
            _currentArrowForce = 0f;
            _currentArrow = null;

        }
 
    }
}
