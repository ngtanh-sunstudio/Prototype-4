using UnityEngine;

public enum BossType
{
    Heavy,
    Sniper
}

public class BossController : MonoBehaviour
{
    public BossType bossType;
    public float speed = 1f;
    private Rigidbody bossRb;
    private GameObject player;

    [Header("Heavy Boss")]
    public GameObject[] enemyPrefabs;
    public float spawnDelay = 1f;
    public float spawnInterval = 3f;
    private float spawnRange = 9f;
    private int enemySpawnCount = 2;

    [Header("Sniper Boss")]
    public GameObject projectile;
    public float fireDelay = 1f;
    public float fireInterval = 1.5f;

    void Awake()
    {
        bossRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (bossType)
        {
            case BossType.Heavy:
                InvokeRepeating("SpawnEnemyWave", spawnDelay, spawnInterval);
                return;
            case BossType.Sniper:
                InvokeRepeating("FireRocket", fireDelay, fireInterval);
                return;
            default:
                return;
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            bossRb.AddForce(lookDirection * speed);
    
            if (transform.position.y < -10)
            {
                Destroy(gameObject);
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

    void SpawnEnemyWave()
    {
        for (int i = 0; i < enemySpawnCount; i++)
        {
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[enemyIndex], GenerateSpawnPosition(),
                        enemyPrefabs[enemyIndex].transform.rotation);
        }
    }

    private void FireRocket()
    {
        GameObject player = GameObject.Find("Player");
        Vector3 fireDirection = (player.transform.position - transform.position).normalized;
        Quaternion fireRotation = Quaternion.FromToRotation(Vector3.up, fireDirection);
        GameObject rocket = Instantiate(projectile, transform.position, fireRotation);
        rocket.GetComponent<RocketProjectile>().Initialize(player.transform, RocketOwner.Boss);
    }
}
