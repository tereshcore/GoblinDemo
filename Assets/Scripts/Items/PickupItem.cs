using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PickupItem : MonoBehaviour
{
    [Header("Game mechanics")]
    [SerializeField] int healingAmount = 30;
    [Header("Drop settings")]
    [SerializeField] private bool isDroped; //Checkbox when it droped from environment
    [SerializeField] private float dropDuration = 1f;
    [SerializeField] private float dropDistance = 1f;
    [SerializeField] private float dropAmplitude = 1f;
    [Header("Wiggle settings")]
    [SerializeField] private float levitateSpeed = 1.0f; // Vertical movement speed
    [SerializeField] private float levitateDistance = 0.2f; // Distance up and down from the initial position
    private float currentTime;
    private Vector3 startPosition;
    private Collider2D collider2d;

    private void Start()
    {
        startPosition = transform.position;
        collider2d = transform.GetComponent<Collider2D>();
        collider2d.enabled = false;
    }

    private void Update()
    {  
        if (!isDroped)
        {
            Wiggle();
        }
        else
        {
            Drop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.GetComponent<Health>();
            if (health.GetHealth() >= health.GetMaxHealth())
            {
                return;
            }
            else
            {
                collision.GetComponent<IDamageable>().TakeDamage(-healingAmount);
                Debug.Log(gameObject + " picked!");
                Destroy(gameObject);
            }
        }
    }

    private void Wiggle()
    {
        Vector3 newPosition = startPosition + new Vector3(0, Mathf.Sin(Time.time * levitateSpeed) * levitateDistance, 0);
        transform.position = newPosition;
    }

    private void Drop()
    {
        currentTime += Time.deltaTime;
        float dropSpeed = dropDistance / dropDuration;
        float x = currentTime * dropSpeed;
        float y = -dropAmplitude * x * (x - dropDistance);

        GameObject player = GameObject.Find("Player");
        Vector3 newPosition = new Vector3(player.transform.localScale.x * x, y, 0); //always drop opposite to player
        transform.position = startPosition + newPosition;

        if (currentTime >= dropDuration)
        {
            startPosition = transform.position;
            isDroped = false;
            collider2d.enabled = true;
        }
    }
}
