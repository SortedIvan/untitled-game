using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowShooting : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _forceMultiplier;
    private GameObject _currentArrow;
    private float _currentArrowForce;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            Quaternion newRotation; /*= Quaternion.Euler(Camera.main.transform.forward);*/
            newRotation = Quaternion.LookRotation(Camera.main.transform.forward);
            _currentArrow = Instantiate(_arrowPrefab, _shootPoint.position, newRotation);
            //_currentArrow = Instantiate(_arrowPrefab);
            _currentArrow.GetComponent<ArrowProjectile>().SetForce(_currentArrowForce);
            _currentArrowForce = 0f;
            _currentArrow = null;

        }
 
    }
}
