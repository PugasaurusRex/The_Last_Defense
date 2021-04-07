using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    public GameObject[] items;
    public int[] mags;
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
        if(Input.GetKeyDown(PlayerInfo.Controls.keys["SwapWeapon"]))
        {
            SwapWeapon();
        }
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
                mags[lastId] = PlayerInfo.ActiveWeaponInfo.mag;
                Destroy(PlayerInfo.ActiveWeapon);
            }

            GameObject temp = Instantiate(items[id]);
            PlayerInfo.ActiveWeapon = temp;
            PlayerInfo.ActiveWeaponInfo = temp.GetComponent<Item>();
            PlayerInfo.ActiveWeaponInfo.mag = mags[id];
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

    public void SwapWeapon()
    {
        bool swap = false;
        if(purchasedItems.Count > 1)
        {
            for(int i = lastId + 1; i < 14; i++)
            {
                if(purchasedItems.Contains(items[i]))
                {
                    SetActiveItem(i);
                    swap = true;
                    break;
                }
            }
            if(!swap)
            {
                SetActiveItem(0);
            }
        }
    }
}
