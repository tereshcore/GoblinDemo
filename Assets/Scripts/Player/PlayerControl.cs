using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int attackDamage;
    [SerializeField]private float attackRate = 2f;
    [SerializeField] private Vector2 attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 input;
    private float nextAttackTime;
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        WalkAnimation();

        input = input.normalized;

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > nextAttackTime)
            {
                anim.SetTrigger("Attack");
                nextAttackTime = Time.time + 1 / attackRate;
            }
        }

    }
    private void FixedUpdate()
    {
        Vector2 moveVector = input * speed * Time.deltaTime;
        rb.position += moveVector;
    }

    private void WalkAnimation()
    {
        if (input.x != 0 || input.y != 0)
        {
            anim.SetBool("Walk", true);

            if (input.x != 0)
            {
                transform.localScale = new Vector3(input.x, 1, 1);
            }
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }

    private void Attack()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCapsuleAll(attackPoint.position, attackRange, CapsuleDirection2D.Vertical, 360, ~playerLayer);

        foreach (Collider2D target in hitTargets)
        {
            Debug.Log(gameObject.name + " hits " + target.name);
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }
}
