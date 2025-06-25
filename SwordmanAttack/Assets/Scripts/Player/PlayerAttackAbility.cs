using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAttackAbility : MonoBehaviour
{
    [SerializeField] private Collider swordDamageCollider;
    [SerializeField] private LayerMask enemyLayer;

    private bool _canAttack = true;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (swordDamageCollider != null)
        {
            swordDamageCollider.enabled = false;
        }
    }

    private void Update()
    {
        if (_canAttack && Input.GetMouseButtonDown(0))
        {
            if (FindEnemyInFront())
            {
                Attack();
            }
        }
    }

    private bool FindEnemyInFront()
    {
        Vector3 playerPos = transform.position;
        Vector3 forward = transform.forward;

        Collider[] hitColliders = Physics.OverlapSphere(playerPos, 2.5f, enemyLayer);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider == null) continue;
            Debug.Log("Detected: " + hitCollider.name);

            Vector3 directionToEnemy = (hitCollider.transform.position - playerPos).normalized;
            float angle = Vector3.Angle(forward, directionToEnemy);

            if (angle < 180f / 2f)
                return true;
        }

        return false;
    }

    private void Attack()
    {
        _canAttack = false;
        _animator.SetLayerWeight(1, 1);
        _animator.SetTrigger("Attack");
    }

    public void EnableSwordCollider()
    {
        if (swordDamageCollider != null)
        {
            swordDamageCollider.enabled = true;
            Debug.Log("Sword Collider Enabled");
        }
    }

    public void DisableSwordCollider()
    {
        if (swordDamageCollider != null)
        {
            swordDamageCollider.enabled = false;
            Debug.Log("Sword Collider Disabled");
        }
    }

    public void ResetAttackCooldown()
    {
        _animator.SetLayerWeight(1, 0);
        _canAttack = true;
        if (swordDamageCollider != null)
        {
            swordDamageCollider.enabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        Gizmos.color = Color.yellow;
        Vector3 forward = transform.forward;
        Vector3 leftLimit = Quaternion.Euler(0, -180f / 2f, 0) * forward * 2.5f;
        Vector3 rightLimit = Quaternion.Euler(0, 180f / 2f, 0) * forward * 2.5f;

        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
    }
}