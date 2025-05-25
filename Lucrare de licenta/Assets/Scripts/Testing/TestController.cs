using UnityEngine;

public class TestController : MonoBehaviour
{
    [Header("Referinte")]
    public GameObject player;
    public GameObject boss;
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public GameObject obstacle;
    public Transform teleportTarget;

    [Header("Damage settings")]
    public int damageAmount = 2;

    void Update()
    {
        // Teleporteaza player-ul la un punct predefinit
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (teleportTarget != null)
            {
                player.transform.position = teleportTarget.position;
                Debug.Log("Player teleportat la target!");
            }
        }

        // Scade HP-ul boss-ului (daca are un script BossHealth)
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (boss != null)
            {
                boss.GetComponent<BossHealth>().TakeDamage(damageAmount);
                Debug.Log("Boss damage cu " + damageAmount);
            }
        }

        // Spawneaza un inamic la pozitia spawnPoint
        if (Input.GetKeyDown(KeyCode.S))
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("Inamic spawnat!");
        }

        // Activeaza / dezactiveaza un obstacol
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (obstacle != null)
            {
                obstacle.SetActive(!obstacle.activeSelf);
                Debug.Log("Obstacle toggled: " + obstacle.activeSelf);
            }
        }

        // Test: Fortam un inamic FSM sa intre in starea Chase
        if (Input.GetKeyDown(KeyCode.C))
        {
            var enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            var ai = enemy.GetComponent<EnemyFSM>();
            if (ai != null)
            {
                ai.ForceChaseState(); // metoda pe care o faci in FSM-ul tau
                Debug.Log("Inamic fortat in Chase!");
            }
        }

        // Test: Fortam un inamic FSM sa intre in starea Attack
        if (Input.GetKeyDown(KeyCode.A))
        {
            var enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            var ai = enemy.GetComponent<EnemyFSM>();
            if (ai != null)
            {
                ai.ForceAttackState(); // metoda pe care o faci in FSM-ul tau
                Debug.Log("Inamic fortat in Attack!");
            }
        }
    }
}