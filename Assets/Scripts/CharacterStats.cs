using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [Header("Vie")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Attaque")]
    public int attackPower = 10;

    [Header("UI")]
    public Slider healthBarSlider;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarSlider != null)
            healthBarSlider.value = (float)currentHealth / maxHealth;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " est mort !");
        // Tu pourras ajouter ici : desactiver le GameObject, jouer une animation de mort, etc.
    }
}
