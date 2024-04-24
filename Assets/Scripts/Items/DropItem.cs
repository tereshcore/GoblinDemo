using UnityEngine;

public class DropItem : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject pickupItemPrefab;
    public void TakeDamage(int damage)
    {
        if (pickupItemPrefab != null)
        {
            GameObject player = GameObject.Find("Player");
            GameObject loot = Instantiate(pickupItemPrefab, transform.position, player.transform.rotation);
        }
        Dead();
    }
    public void Dead()
    {
        Destroy(gameObject);
    }
}
