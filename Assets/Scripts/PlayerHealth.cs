using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 


public class PlayerHealth : MonoBehaviour
{
    // Starting health value for the Player
    public int health = 100;

    // Amount of damage the Player takes when hit
    public int damageAmount = 25;
    public Image healthImage;

    // Reference to the Player's SpriteRenderer (used for flashing red)
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Get the SpriteRenderer component attached to the Player
        spriteRenderer = GetComponent<SpriteRenderer>();

        // NEW: initialize the health bar at start
        UpdateHealthBar();
    }

    // Method to reduce health when damage is taken
    public void TakeDamage()
    {
        health -= damageAmount; // subtract damage amount

        // NEW: update the health bar after taking damage
        UpdateHealthBar();

        StartCoroutine(BlinkRed()); // briefly flash red

        // If health reaches zero or below, call Die()
        if (health <= 0)
        {
            Die();
        }
    }

    // Coroutine to flash the Player red for 0.1 seconds
    private System.Collections.IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    // Reload the scene when the Player dies
    private void Die()
    {
        SceneManager.LoadScene("MainScene");
    }

    // NEW: updates the health bar fill based on current health
    private void UpdateHealthBar()
    {
        if (healthImage != null)
        {
            // health goes from 0–100, convert to 0–1 for fillAmount
            float fillAmount = Mathf.Clamp01((float)health / 100f);
            healthImage.fillAmount = fillAmount;
        }
    }
}
