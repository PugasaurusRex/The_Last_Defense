using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    public GameObject[] items;
    public List<GameObject> purchasedItems = new List<GameObject>();

    public GameObject Player;
    PlayerController PlayerInfo;

    public GameObject ConfirmMenu;
    public TMP_Text CostText;
    public TMP_Text ItemName;

    int lastId = -1;
    int tempId;

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
            ToggleConfirmation(true);
            ItemName.text = items[id].name;
            CostText.text = "" + items[id].GetComponent<Item>().cost;
            tempId = id;
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

    public void PurchaseItem()
    {
        purchasedItems.Add(items[tempId]);
        SetActiveItem(tempId);
        tempId = 0;
    }

    public void ToggleConfirmation(bool visible)
    {
        ConfirmMenu.SetActive(visible);
    }
}
