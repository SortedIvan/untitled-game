using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowTest : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private GameObject _arrowPrefab;

    [Header("Bow")]
    public float grabThreshold = 0.15f;
    public Transform startPoint = null;
    public Transform endPoint = null;
    public Transform arrowSocket = null;
    public float firePowerMultiplier = 10f;
    public float maxFirePower = 10f;
    public float reloadTime = 0.25f;

    private Transform _pullingHand = null;
    private ArrowTest _currentArrow = null;
    private Animator _animator = null;

    private float _pullValue = 0.0f;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        Reload();
    }

    private void Start()
    {
        StartCoroutine(CreateArrow(0.0f));
    }

    private void Update()
    {
        //_pullValue = PullForce();
        this.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        Shoot();
    }

    private void Shoot()
    {
        if (!_currentArrow)
        {
            return;
        }
        float pullForce = 0f;
        if (Input.GetMouseButton(0))
        {
            pullForce += Time.deltaTime * firePowerMultiplier * 10f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            FireArrow(200f);
            Reload();
        }
    }

    private IEnumerator CreateArrow(float waitTime)
    {
        // Wait for a bit
        yield return new WaitForSeconds(waitTime);

        // Create an arrow and child it to point
        GameObject arrow = Instantiate(_arrowPrefab, arrowSocket);
        // Orientate it forward

        arrow.transform.localPosition = new Vector3(0, 0, 5.4f);
        arrow.transform.localEulerAngles = arrowSocket.transform.up;

        // Set the current arrow correctly

        _currentArrow = arrow.GetComponent<ArrowTest>();
    }

    public void Pull(Transform hand)
    {

    }

    public void Reload()
    {
        if (!_currentArrow)
        {
            StartCoroutine(CreateArrow(reloadTime));
        }
    }

    private void FireArrow(float pullForce)
    {
        _currentArrow.FireArrow(pullForce);
        _currentArrow = null;
    }
}
