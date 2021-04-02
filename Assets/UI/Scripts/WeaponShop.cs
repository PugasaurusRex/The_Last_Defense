using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    public GameObject[] items;
    public List<GameObject> purchasedItems = new List<GameObject>();

    public GameObject Player;
    PlayerController PlayerInfo;

    int lastId = -1;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        PlayerInfo = Player.GetComponent<PlayerController>();

        SetActiveItem(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveItem(int id)
    {
        if(!purchasedItems.Contains(items[id]))
        {
            purchasedItems.Add(items[id]);
        }
        else if(id != lastId)
        {
            if (PlayerInfo.ActiveWeapon != null)
            {
                Destroy(PlayerInfo.ActiveWeapon);
            }

            GameObject temp = Instantiate(items[id]);
            PlayerInfo.ActiveWeapon = temp;
            PlayerInfo.ActiveWeaponInfo = temp.GetComponent<Item>();
            lastId = id;
        }
    }
}
