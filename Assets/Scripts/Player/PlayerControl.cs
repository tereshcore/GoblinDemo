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

        MovementAnimation();

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > nextAttackTime)
            {
                anim.SetTrigger("Attack"); //Attack animation contains method Attack() in it
                nextAttackTime = Time.time + 1 / attackRate;
            }
        }

    }
    private void FixedUpdate()
    {
        input = input.normalized;
        Vector2 moveVector = input * speed * Time.deltaTime;
        rb.position += moveVector;
    }

    private void MovementAnimation()
    {
        if (input.x != 0 || input.y != 0)
        {
            anim.SetBool("Walk", true);

            if (input.x != 0 && !Mathf.Approximately(transform.localScale.x, input.x)) //Approximately compares two floating point values and returns true if they are similar. To avoid changing localScale every frame.
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
