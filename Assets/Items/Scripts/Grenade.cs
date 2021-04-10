using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grenade : Item
{
    public GameObject GrenadeObject;
    public int explosiveDamage = 3;
    private List<EnemyController> InRange = new List<EnemyController>();
    Vector3 target;

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
        try
        {
            Speaker.clip = UseSound;
            Speaker.PlayOneShot(Speaker.clip);
        }
        catch
        {
            Debug.Log("No Audio for using item");
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            target = new Vector3(hit.point.x, 1, hit.point.z);
        }

        GameObject temp = Instantiate(GrenadeObject, new Vector3(this.transform.position.x, 1, this.transform.position.z), transform.rotation * Quaternion.Euler(0, 180, 0));
        temp.GetComponent<ProjectileScript>().target = target;
        mag--;
        yield return new WaitForSeconds(rateOfFire);
        PlayerInfo.Anim.SetBool("Shoot", false);
        canUse = true;
    }
}
