using UnityEngine;
namespace TwinTracks
{

    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target; // player
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private float fixedY = 0f; // camera Y

        private float offsetZ;

        private void Start()
        {
            if (target != null)
            {
                // Keep current Z distance between camera and player
                offsetZ = transform.position.z - target.position.z;
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;

            // We only follow X, keep Y fixed
            Vector3 desiredPosition = new Vector3(target.position.x, fixedY, target.position.z + offsetZ);

            // Smooth movement
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}