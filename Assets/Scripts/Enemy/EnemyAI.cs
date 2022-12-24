using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private LayerMask _groundMask;
    private Rigidbody _enemyRb;
    private bool _isOnGround;

    // Enemy patrolling
    [SerializeField] private Vector3 _walkPoint;
    [SerializeField] private float _walkPointRange;
    private bool _walkPointSet;

    // Enemy attacking
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _leapUpPower;
    [SerializeField] private float _leapForwardPower;
    private bool _alreadyAttacked;
    private bool _enemyAttacking;
    private bool _enemyLastAttacked;

    // Enemy states
    [SerializeField] private float _sightRange;
    [SerializeField] private float _attackRange;
    [SerializeField] private bool _playerIsInSightRange;
    [SerializeField] private bool _playerInAttackRange;

    // Enemy status
    [SerializeField] private float _enemyHealth;

    // Enemy friction
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _airDrag;



    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _enemyRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        CheckIsOnGround();
        ApplyFriction();
        // Check for sight and attack range
        //_playerIsInSightRange = Physics.CheckSphere(transform.position, _sightRange, _playerMask);

        if (Vector3.Distance(transform.position, _player.position) < _sightRange)
        {
            _playerIsInSightRange = true;
        }
        else
        {
            _playerIsInSightRange = false;
        }

        if (Vector3.Distance(transform.position, _player.position) < _attackRange && _playerIsInSightRange)
        {
            _playerInAttackRange = true;
        }
        else
        {
            _playerInAttackRange = false;
        }

        if (_playerIsInSightRange && !_playerInAttackRange)
        {
            if (_isOnGround)
            {
                EnemyChase();
            }

        }

        if (_playerInAttackRange && _playerInAttackRange)
        {
            
            EnemyAttack();
        }
    }

    private void EnemyChase()
    {
        ResetAgent();
        this.gameObject.GetComponent<NavMeshAgent>().destination = _player.transform.position;
    }

    private void EnemyAttack()
    {
        //_agent.SetDestination(transform.position);

        transform.LookAt(_player);

        if (!_alreadyAttacked)
        {
            // Attacking here
            LeapTowardsPlayer();
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    public void EnemyTakeDamage(float damage)
    {
        _enemyHealth -= damage;
        if (_enemyHealth <= 0f) Invoke(nameof(KillEnemy), .5f); 
    }

    private void KillEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
    }

    private void LeapTowardsPlayer()
    {
        _enemyRb.isKinematic = false;
        _agent.enabled = false;
        _enemyAttacking = true;
        _enemyRb.AddForce(transform.up * _leapUpPower, ForceMode.Impulse);
        _enemyRb.AddForce(transform.forward * _leapForwardPower, ForceMode.Impulse);
    }

    private void CheckIsOnGround()
    {
        _isOnGround = Physics.Raycast(transform.position, Vector3.down, 2.1f, _groundMask);
    }

    private void ResetAgent()
    {
        if (_enemyAttacking)
        {
            _enemyAttacking = false;
            _agent.enabled = true;
            _enemyRb.isKinematic = true;
        }
    }

    private void ApplyFriction()
    {
        if (_isOnGround)
        {
            _enemyRb.drag = _groundDrag;
        }
        else if (!_isOnGround)
        {
            _enemyRb.drag = _airDrag;

        }
    }

}
