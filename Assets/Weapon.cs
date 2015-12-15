using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public float fireRate = 2.0f;
    public float damage = 10;
    public LayerMask whatToHit;

    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    public Transform BulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    
    float deltaTime = 0.0f;
    Transform firePoint;

	// Use this for initialization
	void Awake () {
        firePoint = transform.FindChild("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No fire point !");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            if (deltaTime >= 1/fireRate)            
            {
                Debug.Log("Shoot");
                Shoot();
                deltaTime = 0f;
            }
        }
        else
        {
            deltaTime += Time.time;
        }

	}

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition-firePointPosition, 100, whatToHit);
        if (Time.time > timeToSpawnEffect)
        {
            StartCoroutine("Effect");
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
        Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100, Color.blue);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            //Debug.Log("We hit " + hit.collider.name + " and did " + damage + " damage");
        }
    }

    IEnumerator Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        Transform clone = (Transform) Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        yield return 0;
        Destroy(clone.gameObject, 0.02f);
    }
}
