using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    void Start()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform SpawnPoint;
    public float spawnDelay = 2f;
    public Transform spawnPrefab;

    public IEnumerator RespawPlayer()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, SpawnPoint.position, SpawnPoint.rotation);
        GameObject clone = (GameObject) Instantiate(spawnPrefab, SpawnPoint.position, SpawnPoint.rotation);
        Destroy(clone, 3f); 
    }

	public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm.RespawPlayer());
    }

    public static void KillEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
