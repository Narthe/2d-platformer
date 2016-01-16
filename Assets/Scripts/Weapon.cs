using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public float fireRate = 2.0f;
    public int damage = 10;
    public LayerMask whatToHit;

    private BulletTrail _bulletTrail;

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
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition-firePointPosition, 100, whatToHit);

        //Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100, Color.blue);
        if (hit.collider != null)
        {
            //Debug.DrawLine(firePointPosition, hit.point, Color.red);
            //Debug.Log("We hit " + hit.collider.name + " and did " + damage + " damage");
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
            }
        }

        if (Time.time > timeToSpawnEffect)
        {
            Debug.Log("time To Spawn");
            Vector3 hitPos;
            if (hit.collider == null)
                hitPos = (mousePosition - firePointPosition) * 30;
            else
                hitPos = hit.point;
            Effect(hitPos);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos)
    {
        Debug.Log("instantiate trail");
        Transform trail = (Transform) Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        _bulletTrail = trail.GetComponent<BulletTrail>();
        if(_bulletTrail != null)
        {
            _bulletTrail.DrawLine(firePoint.position, hitPos, 5f);
        }

        Transform clone = (Transform) Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);
    }
}
