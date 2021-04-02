using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    Rigidbody Rig;
    public float Speed = 8;
    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        Rig = this.GetComponent<Rigidbody>();
        Rig.velocity = this.transform.forward * Speed * -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        if (other.gameObject.name != "Player" && other.gameObject.tag != "Tower" && other.gameObject.tag != "Floor")
        {
            Destroy(this.gameObject);
        }
    }
}
