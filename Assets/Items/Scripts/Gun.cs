using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gun : Item
{
    public GameObject MuzzleFlash;
    public GameObject bulletSpawn;
    public GameObject bullet;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }

    override public IEnumerator Use()
    {
        StartCoroutine(Flash());
        GameObject temp = Instantiate(bullet, new Vector3(bulletSpawn.transform.position.x, 1, bulletSpawn.transform.position.z), this.transform.rotation);
        mag--;
        yield return new WaitForSeconds(rateOfFire);
        PlayerInfo.Anim.SetBool("Shoot", false);
        canUse = true;
    }

    IEnumerator Flash()
    {
        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        MuzzleFlash.SetActive(false);
    }
}
