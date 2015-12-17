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
    public int spawnDelay = 2;

    public IEnumerator RespawPlayer()
    {
        Debug.Log("TODO : add waiting for respawn sound");
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, SpawnPoint.position, SpawnPoint.rotation);
        Debug.Log("TODO : add spwn particules");
    }

	public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm.RespawPlayer());
    }
}
