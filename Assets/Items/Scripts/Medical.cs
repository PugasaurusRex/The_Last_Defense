using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Medical : Item
{
    public bool health = true;
    public bool armor = false;
    public bool stim = false;
    public bool tower = false;

    TowerShop TowerShopRef;
    GoalScript Goal;
    public float distance = 0;
    public int healAmount = 0;
    public float stimLength = 0;

    // Start is called before the first frame update
    override public void Start()
    {
        base.Start();
        TowerShopRef = GameObject.Find("TowerShopUI").GetComponent<TowerShop>();
        Goal = GameObject.Find("Goal").GetComponent<GoalScript>();
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

        if (tower)
        {
            RepairTowers();
        }
        if(health)
        {
            HealPlayer();
        }
        if(armor)
        {
            HealArmor();
        }
        if(stim)
        {
            Stim();
        }

        mag--;
        yield return new WaitForSeconds(rateOfFire);
        PlayerInfo.Anim.SetBool("Shoot", false);
        canUse = true;
    }

    public void RepairTowers()
    {
        foreach (GameObject i in TowerShopRef.PlacedTowers)
        {
            if (i != null)
            {
                if (Vector3.Distance(this.transform.position, i.transform.position) < distance)
                {
                    if(i.GetComponent<TowerController>().health + healAmount <= i.GetComponent<TowerController>().maxHealth)
                    {
                        i.GetComponent<TowerController>().health += healAmount;
                    }
                    else
                    {
                        i.GetComponent<TowerController>().health = i.GetComponent<TowerController>().maxHealth;
                    }
                }
            }
        }
        if(Vector3.Distance(this.transform.position, Goal.gameObject.transform.position) < distance)
        {
            if (Goal.health + healAmount <= Goal.maxHealth)
            {
                Goal.health += healAmount;
            }
            else
            {
                Goal.health = Goal.maxHealth;
            }
        }
    }

    public void HealPlayer()
    {
        if (PlayerInfo.health + healAmount <= 100)
        {
            PlayerInfo.health += healAmount;
        }
        else
        {
            PlayerInfo.health = 100; ;
        }
    }

    public void HealArmor()
    {
        if (PlayerInfo.armor + healAmount <= 100)
        {
            PlayerInfo.armor += healAmount;
        }
        else
        {
            PlayerInfo.armor = 100; ;
        }
    }

    public void Stim()
    {
        PlayerInfo.stimSpeed = healAmount;
        StartCoroutine(StimReset());
    }

    IEnumerator StimReset()
    {
        yield return new WaitForSeconds(stimLength);
        PlayerInfo.stimSpeed = 0;
    }
}
