using UnityEngine;
namespace TwinTracks
{

    public class ObstacleKillZone : MonoBehaviour
    {
        // If true -> obstacle is on the BOTTOM lane
        // If false -> obstacle is on the TOP lane
        [SerializeField] private bool obstacleOnBottom = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Only react to Player
            if (!other.CompareTag("Player"))
                return;

            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement == null)
                return;

            bool playerOnBottom = playerMovement.IsOnBottom;
            bool shouldDie = false;

            // If obstacle is on bottom and player is on bottom -> die
            if (obstacleOnBottom && playerOnBottom)
            {
                shouldDie = true;
            }
            // If obstacle is on top and player is on top -> die
            else if (!obstacleOnBottom && !playerOnBottom)
            {
                shouldDie = true;
            }

            if (shouldDie)
            {
                // 🔊 PLAY DEATH SOUND
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlaySfx(AudioManager.instance.deathSfx);
                }

                // Show Game Over UI
                LevelManager lm = FindObjectOfType<LevelManager>();
                if (lm != null)
                {
                    lm.ShowGameOver();
                }
                else
                {
                    Debug.LogError("ObstacleKillZone: LevelManager not found in scene!");
                }
            }
            else
            {
                Debug.Log("SAFE: Player passed obstacle on " + (obstacleOnBottom ? "BOTTOM" : "TOP") + " lane");
            }
        }
    }
}
