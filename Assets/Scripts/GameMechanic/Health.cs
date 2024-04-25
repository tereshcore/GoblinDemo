using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject deadSkull;
    [SerializeField] private int startHealth;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] bool isPlayer;
    [SerializeField] Slider playerHpSlider;
    [SerializeField] Material hitMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] float damageColorTime = 0.1f;
    private int currentHealth;

    private void Start()
    {
        if (startHealth <= 0)
        {
            startHealth = maxHealth;
        }
        currentHealth = startHealth;
    }

    public int GetHealth()
    {
        return currentHealth;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        Debug.Log(gameObject.name + " HP = " + currentHealth);
        if (damage > 0)
        {
            GetComponent<SpriteRenderer>().material = hitMaterial;
            StartCoroutine("HitTimer");
        }
        if (isPlayer)
        {
            playerHpSlider.value = (float)currentHealth / startHealth;
            Debug.Log("Player HP Slider " + playerHpSlider.value);
        }
        if (currentHealth == 0) 
        {
            Debug.Log(gameObject + " is dead");
            GetComponent<IDamageable>().Dead();
        }
    }

    public void Dead()
    {
        Instantiate(deadSkull, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(damageColorTime);
        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }
}
