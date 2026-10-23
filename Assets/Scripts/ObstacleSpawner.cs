
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject plantPrefab;
    public GameObject pteroPrefab;

    [Header("Spawn Points")]
    public Transform plantSpawnPoint;
    public Transform pteroSpawnPoint;

    [Header("References")]
    public PlayerDash playerDash;
    public Camera gameCamera;

    [Header("Random Spawning")]
    public float firstSpawnDistance = 4f;
    public float minGap = 11f;
    public float maxGap = 17f;

    [Range(0f, 1f)]
    public float plantProbability = 0.5f;

    public float offscreenDistance = 2f;

    private float traveledDistance;
    private float nextSpawnDistance;

    private void Start()
    {
        if (gameCamera == null)
            gameCamera = Camera.main;

        if (plantPrefab == null ||
            pteroPrefab == null ||
            plantSpawnPoint == null ||
            pteroSpawnPoint == null ||
            playerDash == null ||
            gameCamera == null)
        {
            Debug.LogError("ObstacleSpawner: Missing references!");
            enabled = false;
            return;
        }

        nextSpawnDistance = Mathf.Max(0.1f, firstSpawnDistance);
    }

    private void Update()
    {
        traveledDistance +=
            playerDash.CurrentSpeed * Time.deltaTime;

        if (traveledDistance < nextSpawnDistance)
            return;

        traveledDistance -= nextSpawnDistance;

        SpawnObstacle();

        float minimum = Mathf.Max(1f, minGap);
        float maximum = Mathf.Max(minimum, maxGap);

        nextSpawnDistance =
            Random.Range(minimum, maximum);
    }

    private void SpawnObstacle()
    {
        bool spawnPlant =
            Random.value < plantProbability;

        GameObject prefab =
            spawnPlant ? plantPrefab : pteroPrefab;

        Transform spawnPoint =
            spawnPlant ? plantSpawnPoint : pteroSpawnPoint;

        float cameraRight =
            gameCamera.transform.position.x +
            gameCamera.orthographicSize * gameCamera.aspect;

        Vector3 spawnPosition = new Vector3(
            cameraRight + offscreenDistance,
            spawnPoint.position.y,
            0f
        );

        Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}
