using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Goblin : MonoBehaviour, IDamageable
{
    [SerializeField] private float speed;
    [SerializeField] private int attackDamage;
    [SerializeField] private GameObject deadSkull;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;  
    [SerializeField] private float agroZone;
    [SerializeField] private float attackRange;
    private Transform player;
    private Vector3 distance;
    private Animator anim;
    private float directionX;
    private bool isFollow;
    private bool isAttacking;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
        if (player != null)
        {
            distance = (player.position - transform.position);
            isAttacking = Vector2.Distance(attackPoint.position, player.position) < attackRange;

            if (isAttacking)
            {
                anim.SetTrigger("Attack");
            }
            else if (distance.magnitude < agroZone)
            {
                isFollow = true;
            }
            else
            {
                isFollow = false;
            }

        }
    }

    private void FixedUpdate()
    {
        if (isFollow)
        {
            if(!isAttacking)
            {
                distance = distance.normalized;
                transform.position += distance * speed * Time.deltaTime;
                anim.SetBool("Walk", true);
                if (distance.x >= 0)
                {
                    directionX = 1;
                }
                else
                {
                    directionX = -1;
                }
                transform.localScale = new Vector3(directionX, 1, 1);
            }

        }
        else
        { 
            anim.SetBool("Walk", false); 
        }
    }

    private void Attack()
    {
        Collider2D target = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (target != null)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log(gameObject.name + " attacks " + target.name);
                damageable.TakeDamage(attackDamage);
            }
        }

    }

    public void TakeDamage(int damage)
    {
        GetComponent<Health>().TakeDamage(damage);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, agroZone); // Agro Zone visual
        Gizmos.DrawWireSphere(attackPoint.position, attackRange); // Attack Range visual
    }
    public void Dead()
    {
        Instantiate(deadSkull, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
