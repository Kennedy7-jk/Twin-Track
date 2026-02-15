using UnityEngine;
namespace TwinTracks
{

    public class GapKillZone : MonoBehaviour
    {
        // If true -> this gap is on the BOTTOM lane
        // If false -> this gap is on the TOP lane
        [SerializeField] private bool gapOnBottom = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Only care about the player
            if (!other.CompareTag("Player"))
                return;

            // Get the PlayerMovement to know which lane he is on
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement == null)
                return;

            bool playerOnBottom = playerMovement.IsOnBottom;
            bool shouldDie = false;

            // If gap is on bottom and player is on bottom -> die
            if (gapOnBottom && playerOnBottom)
            {
                shouldDie = true;
            }
            // If gap is on top and player is on top -> die
            else if (!gapOnBottom && !playerOnBottom)
            {
                shouldDie = true;
            }

            if (shouldDie)
            {
                // Ask LevelManager to show Game Over UI
                LevelManager lm = FindObjectOfType<LevelManager>();
                if (lm != null)
                {
                    lm.ShowGameOver();
                }
                else
                {
                    Debug.LogError("GapKillZone: LevelManager not found in scene!");
                }
            }
            else
            {
                Debug.Log("SAFE: Player passed gap on " + (gapOnBottom ? "BOTTOM" : "TOP") + " lane");
            }
        }
    }
}