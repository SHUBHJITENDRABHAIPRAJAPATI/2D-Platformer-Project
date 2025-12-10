using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int coinValue = 1;    // ➤ DEFAULT VALUE (Bronze = 1)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Line 7");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Line 11");
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                player.coins += coinValue;   // ➤ ADD CORRECT VALUE
            }

            // SoundManager.Instance.PlaySFX("COIN", 0.4f);  
            Destroy(gameObject);
        }
    }
}
