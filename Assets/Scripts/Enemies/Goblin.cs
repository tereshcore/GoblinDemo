using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Goblin : MonoBehaviour
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
                anim.SetTrigger("Attack"); //Attack animation contains method Attack() in it
            }
            else if (distance.magnitude < agroZone)
            {
                FollowPlayer();
            }
            else
            {
                anim.SetBool("Walk", false);
            }
        }
    }

    private void FollowPlayer()
    {
        int directionX;

        distance = distance.normalized;
        transform.position += distance * speed * Time.deltaTime;
        
        anim.SetBool("Walk", true);
        directionX = (int) Mathf.Sign(distance.x); // return 1 if distance.x > 0 and return -1 if distance.x < 0;
        if (!Mathf.Approximately(transform.localScale.x, directionX)) // Approximately compares two floating point values and returns true if they are similar. To avoid changing localScale every frame.
        {
            transform.localScale = new Vector3(directionX, 1, 1);
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
}
