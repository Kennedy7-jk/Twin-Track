using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TwinTracks
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Forward Movement")]
        [SerializeField] private float moveSpeed = 5f; // Set by LevelManager

        [Header("Lane Positions")]
        [SerializeField] private float bottomY = -3.25f;
        [SerializeField] private float topY = 3.39f;
        [SerializeField] private float verticalOffset = 0.5f;

        [Header("Lane Switch Settings")]
        [SerializeField] private float laneSwitchSpeed = 8f;

        [Header("Player Transform")]
        [SerializeField] private Transform playerTransform;

        [Header("Player Sprites")]
        [SerializeField] private SpriteRenderer playerImage;
        [SerializeField] private List<Sprite> playerSprites;

        private bool isOnBottom = true;
        public bool IsOnBottom => isOnBottom;

        private bool isSwitchingLane = false;
        private float targetY;

        private void Start()
        {
            // Start on bottom lane
            float startY = bottomY + verticalOffset;
            transform.position = new Vector3(transform.position.x, startY, transform.position.z);

            targetY = startY;
            isOnBottom = true;
        }

        private void Update()
        {
            // Auto-run to the right
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            // Start switching on SPACE
            if (Input.GetKeyDown(KeyCode.Space) && !isSwitchingLane)
            {
                StartLaneSwitch();
            }

            // Smooth vertical move
            if (isSwitchingLane)
            {
                Vector3 pos = transform.position;

                pos.y = Mathf.MoveTowards(pos.y, targetY, laneSwitchSpeed * Time.deltaTime);
                transform.position = pos;

                // Reached target
                if (Mathf.Abs(pos.y - targetY) < 0.01f)
                {
                    pos.y = targetY;
                    transform.position = pos;
                    isSwitchingLane = false;
                }
            }
        }

        public void SetPlayerSprite(int playerID)
        {
            Debug.Log($"THE_PLAYER before condition sprite id is {playerID}");

            if (playerSprites == null || playerID > playerSprites.Count || playerImage == null) return;

            Debug.Log($"THE_PLAYER sprite id is {playerID}");

            playerImage.sprite = playerSprites[playerID];
        }

        private void StartLaneSwitch()
        {
            if (isOnBottom)
            {
                isOnBottom = false;
                targetY = topY + verticalOffset;
            }
            else
            {
                isOnBottom = true;
                targetY = bottomY + verticalOffset;
            }

            // Rotate player visually
            SetPlayerPosition(isOnBottom);

            // 🔊 PLAY JUMP / LANE SWITCH SOUND
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySfx(AudioManager.instance.jumpSfx);
            }

            isSwitchingLane = true;
        }

        public void SetPlayerPosition(bool isInBottom)
        {
            playerTransform.eulerAngles = isInBottom ? new Vector3(0f, 180f, 0f) : new Vector3(180f, 180f, 0f);
        }

        // ⭐ Called by LevelManager to change speed per level
        public void SetMoveSpeed(float newSpeed)
        {
            moveSpeed = newSpeed;
        }
    }
}

