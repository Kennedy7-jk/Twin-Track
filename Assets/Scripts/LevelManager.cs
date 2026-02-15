using UnityEngine;
using UnityEngine.UI;
namespace TwinTracks
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Transform player;

        [Header("Levels")]
        [SerializeField] private GameObject[] levels;
        [SerializeField] private Transform[] levelStartPoints;

        [Header("Level Speeds")]
        [SerializeField] private float[] levelMoveSpeeds;

        [Header("UI Panels")]
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private GameObject gameOverPanel;

        [Header("Stars UI (Text)")]
        [SerializeField] private Text levelCompleteStarsText;

        [Header("Stars Images")]
        [SerializeField] private Image firstStar;
        [SerializeField] private Image secondStar;
        [SerializeField] private Image thirdStar;

        private int currentLevelIndex = 0;

        // Coin tracking
        private int currentCoins = 0;
        private int maxCoinsInLevel = 0;

        private void Start()
        {
            // Hide UI panels
            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(false);

            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            // Disable all levels
            if (levels != null)
            {
                foreach (GameObject lvl in levels)
                {
                    if (lvl != null)
                        lvl.SetActive(false);
                }
            }

            // Disable player
            if (player != null)
                player.gameObject.SetActive(false);

            // Show main menu
            if (mainMenuUI != null)
                mainMenuUI.SetActive(true);


            Time.timeScale = 0f;

            Debug.Log("LevelManager: Game started in MAIN MENU state.");
        }

        // ========================= LEVEL CONTROL ==========================

        public void LoadLevel(int levelIndex)
        {

            if (levels == null || levelStartPoints == null)
            {
                Debug.LogError("LevelManager: Levels or LevelStartPoints NOT assigned!");
                return;
            }

            if (levelIndex < 0 || levelIndex >= levels.Length)
            {
                Debug.LogError("LevelManager: Invalid level index " + levelIndex);
                return;
            }

            // Disable all levels
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i] != null)
                    levels[i].SetActive(false);

            }

            Debug.LogError("LevelManager: Current Level " + levelIndex);

            // Enable selected level
            GameObject levelRoot = levels[levelIndex];
            if (levelRoot != null)
                levelRoot.SetActive(true);

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayBgm(levelIndex);
            }

            currentLevelIndex = levelIndex;

            // Move player to start point
            if (player != null &&
                levelIndex < levelStartPoints.Length &&
                levelStartPoints[levelIndex] != null)
            {
                player.position = levelStartPoints[levelIndex].position;
            }

            // Set movement speed + reset lane
            if (player != null)
            {
                PlayerMovement pm = player.GetComponent<PlayerMovement>();
                if (pm != null && levelMoveSpeeds != null && levelIndex < levelMoveSpeeds.Length)
                {
                    pm.SetMoveSpeed(levelMoveSpeeds[levelIndex]);
                    pm.SetPlayerPosition(true);
                    pm.SetPlayerSprite(levelIndex);
                }
            }

            // Reset coins
            currentCoins = 0;
            maxCoinsInLevel = 0;

            // Count coins in this level
            if (levelRoot != null)
            {
                Coin[] coins = levelRoot.GetComponentsInChildren<Coin>(true);
                maxCoinsInLevel = coins.Length;

                foreach (Coin coin in coins)
                {
                    coin.gameObject.SetActive(true);
                }

                Debug.Log($"Coins in this level: {maxCoinsInLevel}");
            }

            // Reset star visuals
            ResetStars();

            // Hide panels
            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(false);

            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
        }

        public void LoadNextLevel()
        {

            if (AudioManager.instance != null)
            {
                AudioManager.instance.StopBgm();
            }

            int nextIndex = currentLevelIndex + 1;

            if (levels != null && nextIndex < levels.Length)
            {
                LoadLevel(nextIndex);
            }
            else
            {
                ShowMainMenu();
            }
        }

        // ========================= MAIN MENU ==========================

        public void OnStartButton()
        {
            if (player != null)
                player.gameObject.SetActive(true);

            LoadLevel(0);

            if (mainMenuUI != null)
                mainMenuUI.SetActive(false);

            Time.timeScale = 1f;
        }

        public void ShowMainMenu()
        {
            if (levels != null)
            {
                foreach (GameObject lvl in levels)
                {
                    if (lvl != null)
                        lvl.SetActive(false);
                }
            }

            if (player != null)
                player.gameObject.SetActive(false);

            if (mainMenuUI != null)
                mainMenuUI.SetActive(true);

            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(false);

            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            Time.timeScale = 0f;
        }

        // ========================= LEVEL COMPLETE ==========================

        public void ShowLevelComplete()
        {
            Time.timeScale = 0f;

            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(true);

            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            UpdateLevelCompleteStarsUI();
        }

        public void OnNextLevelButton()
        {
            Time.timeScale = 1f;
            LoadNextLevel();
        }

        public void OnRestartButton()
        {
            Time.timeScale = 1f;
            LoadLevel(currentLevelIndex);
        }

        // ========================= GAME OVER ==========================

        public void ShowGameOver()
        {
            Time.timeScale = 0f;

            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(false);
        }

        public void OnDeathRestartButton()
        {
            Time.timeScale = 1f;
            LoadLevel(currentLevelIndex);
        }

        public void OnQuitToMenuButton()
        {
            ShowMainMenu();
        }

        // ========================= COINS & STARS ==========================

        public void OnCoinCollected()
        {
            currentCoins++;
        }

        private void UpdateLevelCompleteStarsUI()
        {
            int stars = CalculateStars();

            if (levelCompleteStarsText != null)
            {
                levelCompleteStarsText.text = $"Stars: {stars} / 3";
            }

            if (stars >= 1) firstStar.gameObject.SetActive(true);
            if (stars >= 2) secondStar.gameObject.SetActive(true);
            if (stars >= 3) thirdStar.gameObject.SetActive(true);
        }

        private void ResetStars()
        {
            if (firstStar != null) firstStar.gameObject.SetActive(false);
            if (secondStar != null) secondStar.gameObject.SetActive(false);
            if (thirdStar != null) thirdStar.gameObject.SetActive(false);
        }

        private int CalculateStars()
        {
            if (maxCoinsInLevel <= 0)
                return 3;

            if (currentCoins <= 0)
                return 0;

            if (currentCoins == maxCoinsInLevel)
                return 3;

            float ratio = (float)currentCoins / maxCoinsInLevel;

            if (ratio >= 0.5f)
                return 2;

            return 1;
        }
    }
}