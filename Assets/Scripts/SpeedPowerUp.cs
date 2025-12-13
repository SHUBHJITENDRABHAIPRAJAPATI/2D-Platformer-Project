using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    public float speedMultiplier = 2f;
    public float duration = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                player.ActivateSpeedBoost(speedMultiplier, duration);
                SoundManager.Instance.PlaySFX("POWERUP"); // 🔊 NEW
            }

            Destroy(gameObject);
        }
    }
}
