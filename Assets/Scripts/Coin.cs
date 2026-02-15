using UnityEngine;
namespace TwinTracks
{
    public class Coin : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            // Play coin sound
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySfx(AudioManager.instance.coinSfx);
            }

            // Inform LevelManager
            LevelManager lm = FindObjectOfType<LevelManager>();
            if (lm != null)
            {
                lm.OnCoinCollected();
            }

            gameObject.SetActive(false);
        }
    }
}