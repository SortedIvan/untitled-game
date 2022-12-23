using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private LayerMask _groundMask;

    // Enemy patrolling
    [SerializeField] private Vector3 _walkPoint;
    [SerializeField] private float _walkPointRange;
    private bool _walkPointSet;

    // Enemy attacking
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private GameObject _projectile;
    private bool _alreadyAttacked;

    // Enemy states
    [SerializeField] private float _sightRange;
    [SerializeField] private float _attackRange;
    [SerializeField] private bool _playerIsInSightRange;
    [SerializeField] private bool _playerInAttackRange;

    // Enemy status
    [SerializeField] private float _enemyHealth;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check for sight and attack range
        _playerIsInSightRange = Physics.CheckSphere(transform.position, _sightRange, _playerMask);
        
        if (Physics.CheckSphere(transform.position, _attackRange, _playerMask))
        {
            Debug.Log("Player in range somehow");
            _playerInAttackRange = true;
        }
        else
        {
            _playerInAttackRange = false;
        }
        
        if (_playerIsInSightRange && !_playerInAttackRange)
        {
            EnemyChase();
        }

        if (_playerInAttackRange && _playerInAttackRange)
        {
            EnemyAttack();
        }
    }


    private void SearchForWalkPoint()
    {
        float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _groundMask))
        {
            _walkPointSet = true;
        }
    }

    private void EnemyChase()
    {
        //_agent.SetDestination(_player.position);
        this.gameObject.GetComponent<NavMeshAgent>().destination = _player.transform.position;
    }

    private void EnemyAttack()
    {
        _agent.SetDestination(transform.position);

        transform.LookAt(_player);

        if (!_alreadyAttacked)
        {
            // Attacking here
            //Rigidbody rb = Instantiate(_projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    public void EnemyTakeDamage(int damage)
    {
        _enemyHealth -= damage;
        if (_enemyHealth <= 0) Invoke(nameof(KillEnemy), .5f); 
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
}
