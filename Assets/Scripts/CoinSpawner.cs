
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Prefab")]
    public GameObject coinPrefab;

    [Header("References")]
    public Transform coinSpawnPoint;
    public PlayerDash playerDash;
    public Camera gameCamera;

    [Header("Spawn Settings")]
    public float firstSpawnDistance = 7f;
    public float minGap = 12f;
    public float maxGap = 20f;
    public float offscreenDistance = 2f;

    private float remainingDistance;

    private void Start()
    {
        if (gameCamera == null)
            gameCamera = Camera.main;

        if (coinPrefab == null ||
            coinSpawnPoint == null ||
            playerDash == null ||
            gameCamera == null)
        {
            Debug.LogError(
                "CoinSpawner: Missing references!"
            );

            enabled = false;
            return;
        }

        remainingDistance =
            Mathf.Max(0.1f, firstSpawnDistance);
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        remainingDistance -=
            playerDash.CurrentSpeed * Time.deltaTime;

        if (remainingDistance > 0f)
            return;

        SpawnCoin();

        float minimum = Mathf.Max(1f, minGap);
        float maximum = Mathf.Max(minimum, maxGap);

        remainingDistance +=
            Random.Range(minimum, maximum);
    }

    private void SpawnCoin()
    {
        float cameraRight =
            gameCamera.transform.position.x +
            gameCamera.orthographicSize *
            gameCamera.aspect;

        Vector3 position = new Vector3(
            cameraRight + offscreenDistance,
            coinSpawnPoint.position.y,
            0f
        );

        Instantiate(
            coinPrefab,
            position,
            Quaternion.identity
        );
    }
}
