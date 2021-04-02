using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    // Store All Enemies In Range of The Tower.
    public List<GameObject> InRange = new List<GameObject>();

    public GameObject Wave;
    public WaveController WaveInfo;

    // GameState Variables
    public int health = 100;

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

    private int i = 0;
    private bool CanShoot = true;
    public bool useProjectile = true;

    // Start is called before the first frame update
    void Start()
    {
        Wave = GameObject.Find("WaveController");
        WaveInfo = Wave.GetComponent<WaveController>();
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
                    if (i >= InRange.Count)
                    {
                        i = 0;
                    }

                    if (InRange[i] == null)
                    {
                        InRange.RemoveAt(i);
                        i = 0;
                    }

                    if (i < InRange.Count && InRange[i] != null)
                    {
                        if (Vector3.Distance(this.transform.position, InRange[i].transform.position) > range)
                        {
                            InRange.RemoveAt(i);
                        }
                        else if (Vector3.Distance(this.transform.position, InRange[i].transform.position) < .5f)
                        {
                            target = InRange[i].gameObject;
                        }
                        else if (Physics.Raycast(this.transform.position, InRange[i].gameObject.transform.position - this.transform.position, out RaycastHit hit) && hit.collider.gameObject == InRange[i].gameObject)
                        {
                            target = InRange[i].gameObject;
                        }
                        else
                        {
                            target = null;
                            i++;
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

                if (target != null)
                {
                    rotator.transform.LookAt(target.transform.position);

                    if (CanShoot)
                    {
                        CanShoot = false;
                        MuzzleFlash.SetActive(true);
                        StartCoroutine(Flash());
                        StartCoroutine(Shoot());
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
        yield return new WaitForSeconds(.05f);
        MuzzleFlash.SetActive(false);
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }
}
