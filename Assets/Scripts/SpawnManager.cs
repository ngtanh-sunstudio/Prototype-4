using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] bossPrefabs;
    public GameObject powerupPrefab;
    public GameObject rocketPowerupPrefab;
    public float rocketProbability = 0.25f;
    private float spawnRange = 9f;
    public int enemyCount;
    public int waveNumber = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        SpawnPowerup();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            SpawnPowerup();
            if (waveNumber % 5 == 0)
            {
                SpawnBoss();
            }
        }
    }


    private Vector3 GenerateSpawnPosition()
    {
        float spawnPointX = Random.Range(-spawnRange, spawnRange);
        float spawnPointZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPointX, 0, spawnPointZ);
        return randomPos;
    }

    void SpawnEnemyWave(int enemyToSpawn)
    {
        for (int i = 0; i < enemyToSpawn; i++)
        {
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[enemyIndex], GenerateSpawnPosition(),
                        enemyPrefabs[enemyIndex].transform.rotation);
        }
    }
    
    void SpawnBoss()
    {
        int bossIndex = Random.Range(0, bossPrefabs.Length);
        Instantiate(bossPrefabs[bossIndex], GenerateSpawnPosition(),
                    bossPrefabs[bossIndex].transform.rotation);
    }

    void SpawnPowerup()
    {
        Instantiate(powerupPrefab, GenerateSpawnPosition(),
                    powerupPrefab.transform.rotation);

        if (Random.Range(0f, 1f) <= rocketProbability)
        {
            Instantiate(rocketPowerupPrefab, GenerateSpawnPosition(),
                        rocketPowerupPrefab.transform.rotation);
        }
    }
}
