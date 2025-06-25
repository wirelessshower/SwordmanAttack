using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private float startingHealth = 50f;

    private float currentHealth;
    private float maxHealth;

    private void Start()
    {
        currentHealth = maxHealth = startingHealth;
        UpdateHealthBar();
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsSword(other))
            TakeDamage(PlayerStats.Instance.GetDamage());
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);
        UpdateHealthBar();

        if (currentHealth == 0f)
            Die();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.fillAmount = currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
        // Здесь можешь вызывать выдачу опыта
    }

    private bool IsSword(Collider other)
    {
        if (other.CompareTag("Sword"))
            return true;
        return false;
    }
    
}