using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    public GameObject[] items;
    public Image[] shopImages;
    public int[] mags;
    public List<GameObject> purchasedItems = new List<GameObject>();

    GameObject Player;
    PlayerController PlayerInfo;

    public GameObject ErrorMenu;
    public GameObject ConfirmMenu;
    public TMP_Text CostText;
    public TMP_Text ItemName;

    int lastId = -1;
    int tempId;

    AudioSource Speaker;
    public AudioClip SelectWeaponSound;
    public AudioClip BuyWeaponSound;
    public AudioClip CancelSound;

    public GameObject ShopInfoPanel;

    public TMP_Text CostInfoText;
    public TMP_Text DamageText;
    public TMP_Text ReloadText;
    public TMP_Text AccuracyText;
    public TMP_Text FireRateText;
    public TMP_Text HealText;
    public TMP_Text DescriptionText;
    public TMP_Text NameText;

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();
        Speaker.volume = PlayerPrefs.GetFloat("volume", 1);

        Player = GameObject.Find("Player");
        PlayerInfo = Player.GetComponent<PlayerController>();

        shopImages[0].color = Color.yellow;
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
            Speaker.clip = SelectWeaponSound;
            Speaker.PlayOneShot(Speaker.clip);

            if (PlayerInfo.ActiveWeapon != null)
            {
                mags[lastId] = PlayerInfo.ActiveWeaponInfo.mag;
                Destroy(PlayerInfo.ActiveWeapon);
            }

            GameObject temp = Instantiate(items[id]);
            PlayerInfo.ActiveWeapon = temp;
            PlayerInfo.ActiveWeaponInfo = temp.GetComponent<Item>();
            PlayerInfo.ActiveWeaponInfo.mag = mags[id];

            if(lastId >= 0)
            {
                shopImages[lastId].color = Color.yellow;
            }
            shopImages[id].color = Color.red;

            lastId = id;
        }
    }

    public void PurchaseItem()
    {
        Speaker.clip = BuyWeaponSound;
        Speaker.PlayOneShot(Speaker.clip);

        if(PlayerInfo.scrap >= items[tempId].GetComponent<Item>().cost)
        {
            PlayerInfo.scrap -= items[tempId].GetComponent<Item>().cost;
            purchasedItems.Add(items[tempId]);
            SetActiveItem(tempId);
            tempId = 0;
        }
        else
        {
            ErrorMenu.GetComponentInChildren<TMP_Text>().text = "Not enough scrap.";
            StartCoroutine(ErrorMessage());
        }
    }

    public void ToggleConfirmation(bool visible)
    {
        Speaker.clip = CancelSound;
        Speaker.PlayOneShot(Speaker.clip);

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

    IEnumerator ErrorMessage()
    {
        ErrorMenu.SetActive(true);
        yield return new WaitForSeconds(2f);
        ErrorMenu.SetActive(false);
    }

    public void SetShopText(int id)
    {
        CostInfoText.text = "" + items[id].GetComponent<Item>().cost;
        DamageText.text = "" + items[id].GetComponent<Item>().damage;
        ReloadText.text = "" + items[id].GetComponent<Item>().reloadTime;
        AccuracyText.text = "" + items[id].GetComponent<Item>().accuracy;
        FireRateText.text = "" + items[id].GetComponent<Item>().rateOfFire;
        HealText.text = "" + items[id].GetComponent<Item>().healAmount;
        DescriptionText.text = "" + items[id].GetComponent<Item>().description;
        NameText.text = "" + items[id].GetComponent<Item>().name;
    }

    public void ToggleShopInfo(bool toggle)
    {
        ShopInfoPanel.SetActive(toggle);
    }
}
