using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    // Store All Enemies In Range of The Tower.
    public List<GameObject> InRange = new List<GameObject>();

    GameObject Wave;
    WaveController WaveInfo;

    // GameState Variables
    public int health = 100;
    public int maxHealth;

    // Tower Variables
    public bool shootFlying = false;
    public bool shootGround = true;

    public int damage = 10;
    public float range = 10;
    public float fireRate = 1f;
    public float accuracy = .8f;
    private int accCheck;
    private GameObject target;
    public GameObject rotator;
    public GameObject Projectile;
    public GameObject MuzzleFlash;

    private bool CanShoot = true;
    public bool useProjectile = true;
    public GameObject BulletSpawn;

    // Attack Line
    public int segments = 50;
    public LineRenderer line;

    public Image healthDisplay;

    // Audio
    AudioSource Speaker;
    public AudioClip DieSound;
    public AudioClip ShootSound;
    public AudioClip TakeDamageSound;

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();

        Wave = GameObject.Find("WaveController");
        WaveInfo = Wave.GetComponent<WaveController>();
        maxHealth = health;

        // Create range line
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        line.startWidth = .2f;
        CreatePoints();
        line.enabled = GameObject.Find("TowerShopUI").GetComponent<TowerShop>().lineVisible;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            healthDisplay.fillAmount = (float)health / maxHealth;
            healthDisplay.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

            if (health <= 0)
            {
                GameObject.Find("TowerShopUI").GetComponent<TowerShop>().PlacedTowers.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                if (InRange.Count > 0)
                {
                    if (InRange[0] == null || Vector3.Distance(this.transform.position, InRange[0].transform.position) > range)
                    {
                        InRange.RemoveAt(0);
                        target = null;
                    }
                    else if(Physics.Raycast(BulletSpawn.transform.position, (InRange[0].transform.position - BulletSpawn.transform.position).normalized, out RaycastHit hit))
                    {
                        if (Mathf.Abs(hit.distance - Vector3.Distance(InRange[0].transform.position, BulletSpawn.transform.position)) < 3f)
                        {
                            target = InRange[0].gameObject;
                            rotator.transform.LookAt(new Vector3(target.transform.position.x, rotator.transform.position.y, target.transform.position.z));
                            if (CanShoot)
                            {
                                CanShoot = false;
                                StartCoroutine(Flash());
                                StartCoroutine(Shoot());
                            }
                        }
                        else
                        {
                            InRange.RemoveAt(0);
                            target = null;
                        }
                    }
                }
                else if (WaveInfo.inWave)
                {
                    foreach (GameObject i in WaveInfo.AliveEnemies)
                    {
                        if (Vector3.Distance(this.transform.position, i.transform.position) <= range)
                        {
                            bool temp = i.GetComponent<EnemyController>().flying;
                            if (temp && shootFlying)
                            {
                                InRange.Add(i);
                            }
                            else if(!temp && shootGround)
                            {
                                InRange.Add(i);
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator Shoot()
    {
        if (useProjectile)
        {
            GameObject temp = Instantiate(Projectile, new Vector3(BulletSpawn.transform.position.x, 1, BulletSpawn.transform.position.z), rotator.transform.rotation);
            temp.GetComponent<ProjectileScript>().damage = damage;
            temp.GetComponent<ProjectileScript>().range = range;
            temp.GetComponent<ProjectileScript>().target = target.transform.position;
        }
        else
        {
            accCheck = Random.Range(0, 100);
            if (accCheck <= accuracy * 100)
            {
                EnemyController temp = target.GetComponent<EnemyController>();
                temp.TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(fireRate);
        CanShoot = true;
    }

    IEnumerator Flash()
    {
        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        MuzzleFlash.SetActive(false);
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }

    void CreatePoints()
    {
        float x;
        float z;

        float angle = 20f;

        if(this.transform.lossyScale.x < 1)
        {
            for (int i = 0; i < (segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * range * 4;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * range * 4;

                line.SetPosition(i, new Vector3(x, .5f, z));

                angle += (360f / segments);
            }
        }
        else
        {
            for (int i = 0; i < (segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * range;

                line.SetPosition(i, new Vector3(x, .5f, z));

                angle += (360f / segments);
            }
        }
    }
}
