using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public int damageAmount = 25;
    public Image healthImage;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHealthBar();
    }

    public void TakeDamage()
    {
        health -= damageAmount;          // subtract damage amount
        UpdateHealthBar();               // update the health bar

        SoundManager.Instance.PlaySFX("HURT");   // 🔊 NEW

        StartCoroutine(BlinkRed());      // briefly flash red

        if (health <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void UpdateHealthBar()
    {
        if (healthImage != null)
        {
            float fillAmount = Mathf.Clamp01((float)health / 100f);
            healthImage.fillAmount = fillAmount;
        }
    }
}
