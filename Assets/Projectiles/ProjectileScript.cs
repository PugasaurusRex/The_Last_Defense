using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    Rigidbody Rig;
    public float speed = 8;
    public int damage = 1;
    public int explosiveDamage = 3;
    public float fusetime = 0;
    public float stuntime = 3;
    public float range;
    public float explodeRadius = 5;

    public bool explosive = false;
    public bool grenade = false;
    public bool fused = false;
    public bool stun = false;
    bool used = false;

    public Vector3 target;
    Vector3 spawn;

    private List<EnemyController> InRange = new List<EnemyController>();

    AudioSource Speaker;
    public AudioClip ExplodeSound;

    public GameObject ExplodeParticles;

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();
        Speaker.volume = PlayerPrefs.GetFloat("volume", 1);

        Rig = this.GetComponent<Rigidbody>();
        Rig.velocity = this.transform.forward * speed;
        spawn = this.transform.position;

        if(fused)
        {
            StartCoroutine(Fuse());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!used && grenade && ((Vector3.Distance(this.transform.position, target) < 1f || Vector3.Distance(this.transform.position, spawn) > range)))
        {
            used = true;
            Rig.velocity = Vector3.zero;
            if(!fused)
            {
                Explode();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if(!explosive)
            {
                other.GetComponent<EnemyController>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
            else
            {
                if (!used && other.gameObject.tag == "Enemy")
                {
                    other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
                }
                if (!used && other.gameObject.tag != "Player" && other.gameObject.tag != "Floor" && other.gameObject.tag != "Tower")
                {
                    Explode();
                    Rig.velocity = Vector3.zero;
                }
            }
        }
        if (!explosive && other.gameObject.tag != "Player" && other.gameObject.tag != "Tower" && other.gameObject.tag != "Floor")
        {
            Destroy(this.gameObject);
        }
    }

    public void Explode()
    {
        used = true;

        try
        {
            Speaker.clip = ExplodeSound;
            Speaker.PlayOneShot(Speaker.clip);

            ExplodeParticles.SetActive(true);
            ExplodeParticles.GetComponent<ParticleSystem>().Play();

            GetComponent<MeshRenderer>().enabled = false;
        }
        catch
        {
            Debug.Log("No Audio or Particle for explode.");
        }

        try
        {
            foreach(EnemyController i in FindObjectsOfType<EnemyController>())
            {
                if(Vector3.Distance(i.gameObject.transform.position, this.transform.position) < explodeRadius)
                {
                    if (i != null)
                    {
                        i.TakeDamage(explosiveDamage);

                        if(stun)
                        {
                            i.StartCoroutine(i.Stun(stuntime));
                        }
                    }
                }
            }
        }
        catch
        {
            Debug.Log("Failed to find enemies.");
        }

        StartCoroutine(DestroySelf());
    }

    IEnumerator Fuse()
    {
        yield return new WaitForSeconds(fusetime);
        Explode();
    }

    IEnumerator DestroySelf()
    {
        Collider[] Temp = GetComponents<Collider>();
        foreach(Collider i in Temp)
        {
            i.enabled = false;
        }

        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
