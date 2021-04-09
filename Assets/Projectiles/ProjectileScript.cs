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

    public bool explosive = false;
    public bool grenade = false;
    public bool fused = false;
    public bool stun = false;

    public Vector3 target;
    Vector3 spawn;

    private List<EnemyController> InRange = new List<EnemyController>();

    AudioSource Speaker;
    public AudioClip ExplodeSound;

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();

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
        if (grenade && ((Vector3.Distance(this.transform.position, target) < .5f || Vector3.Distance(this.transform.position, spawn) > range)))
        {
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
                InRange.Add(other.GetComponent<EnemyController>());
            }
        }
        if (!explosive && other.gameObject.tag != "Player" && other.gameObject.tag != "Tower" && other.gameObject.tag != "Floor")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(explosive && other.gameObject.tag == "Enemy" && InRange.Contains(other.GetComponent<EnemyController>()))
        {
            InRange.Remove(other.GetComponent<EnemyController>());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            collision.collider.GetComponent<EnemyController>().TakeDamage(damage);
        }
        if(collision.collider.tag != "Player" && collision.collider.tag != "Tower" && collision.collider.tag != "Floor")
        {
            Explode();
            Rig.velocity = Vector3.zero;
        }
    }

    public void Explode()
    {
        Speaker.clip = ExplodeSound;
        Speaker.PlayOneShot(Speaker.clip);

        if (stun)
        {
            foreach (EnemyController i in InRange)
            {
                i.StartCoroutine(i.Stun(stuntime));
                i.TakeDamage(explosiveDamage);
            }
        }
        else
        {
            foreach (EnemyController i in InRange)
            {
                i.TakeDamage(explosiveDamage);
            }
        }
        Destroy(this.gameObject);
    }

    IEnumerator Fuse()
    {
        yield return new WaitForSeconds(fusetime);
        Explode();
    }
}
