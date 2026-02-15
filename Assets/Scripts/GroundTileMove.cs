using UnityEngine;

public class GroundTileMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 5f;

    private void Update()
    {
        // Move tile to the left
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // If tile is far left, destroy it
        if (transform.position.x < -30f)
        {
            Destroy(gameObject);
        }
    }
}