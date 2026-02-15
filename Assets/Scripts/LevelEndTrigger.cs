using UnityEngine;
namespace TwinTracks
{

    public class LevelEndTrigger : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Only react when Player touches the end trigger
            if (!other.CompareTag("Player"))
                return;

            // ?? PLAY LEVEL COMPLETE SOUND
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySfx(AudioManager.instance.levelCompleteSfx);
            }

            // Show Level Complete UI
            if (levelManager != null)
            {
                levelManager.ShowLevelComplete();
            }
            else
            {
                Debug.LogError("LevelEndTrigger: LevelManager reference not set!");
            }
        }
    }
}