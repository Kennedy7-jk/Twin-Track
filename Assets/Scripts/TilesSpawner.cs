using UnityEngine;
namespace TwinTracks
{
    public class TileSpawner : MonoBehaviour
    {
        [Header("Tile Settings")]
        public GameObject[] tilePrefabs;   // Normal, GapBottom, GapTop
        public float tileWidth = 10f;      // same as platform width
        public int initialTileCount = 15;  // how many tiles in a row

        private void Start()
        {
            float spawnX = 0f;

            for (int i = 0; i < initialTileCount; i++)
            {
                int index = Random.Range(0, tilePrefabs.Length);
                GameObject selectedPrefab = tilePrefabs[index];

                Vector3 spawnPos = new Vector3(spawnX, 0f, 0f);
                GameObject tile = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

                // DEBUG: print what we spawned
                Debug.Log("Spawned tile: " + selectedPrefab.name + " at X = " + spawnX);

                spawnX += tileWidth;
            }
        }
    }
}
