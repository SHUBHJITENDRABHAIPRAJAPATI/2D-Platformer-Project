using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                player.coins += 1;
            }

            SoundManager.Instance.PlaySFX("COIN", 0.4f);   // 🔊 NEW – 40% volume
            Destroy(gameObject);
        }
    }
}
