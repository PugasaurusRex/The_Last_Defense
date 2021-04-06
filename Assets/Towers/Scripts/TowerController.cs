using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Attack Line
    public int segments = 50;
    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        Wave = GameObject.Find("WaveController");
        WaveInfo = Wave.GetComponent<WaveController>();
        maxHealth = health;

        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        line.startWidth = .2f;
        CreatePoints();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            if (health <= 0)
            {
                GameObject.Find("TowerShopUI").GetComponent<TowerShop>().PlacedTowers.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                if (InRange.Count > 0)
                {
                    if (InRange[0] == null)
                    {
                        InRange.RemoveAt(0);
                        target = null;
                    }
                    else if (Vector3.Distance(this.transform.position, InRange[0].transform.position) > range)
                    {
                        InRange.RemoveAt(0);
                        target = null;
                    }
                    else if(target == null)
                    {
                        target = InRange[0].gameObject;
                    }
                    else
                    {
                        rotator.transform.LookAt(new Vector3(target.transform.position.x, rotator.transform.position.y, target.transform.position.z));

                        if (CanShoot)
                        {
                            CanShoot = false;
                            StartCoroutine(Flash());
                            StartCoroutine(Shoot());
                        }
                    }
                }
                else if (WaveInfo.inWave)
                {
                    foreach (GameObject i in WaveInfo.AliveEnemies)
                    {
                        if (Vector3.Distance(this.transform.position, i.transform.position) <= range)
                        {
                            InRange.Add(i);
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
            Instantiate(Projectile, new Vector3(rotator.transform.position.x, 1, rotator.transform.position.z), rotator.transform.rotation);
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

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * range;

            line.SetPosition(i, new Vector3(x, .5f, z));

            angle += (360f / segments);
        }
    }
}
