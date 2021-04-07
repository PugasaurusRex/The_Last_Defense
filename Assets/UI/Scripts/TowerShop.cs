using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TowerShop : MonoBehaviour
{
    public GameObject Player;
    public GameObject WaveControl;
    WaveController wave;
    public GameObject Settings;
    SettingsController Controls;
    Menu MenuSettings;

    public GameObject ErrorMenu;

    public float PlaceDistance = 5f;
    public float PlayerDistance = 15f;

    public List<GameObject> PlacedTowers = new List<GameObject>();

    PlayerController PlayerInfo;

    public bool lineVisible = false;

    // Tower Prices
    public int RiflePrice = 100;
    public int SniperPrice = 100;
    public int MachinegunPrice = 100;
    public int MissilePrice = 100;
    public int AirDefensePrice = 100;
    public int MortarPrice = 100;

    // Temp Towers
    public GameObject TempTower;
    int TempTowerCost = 0;
    int TowerId = 0;

    public GameObject ShopRifle;
    public GameObject ShopSniper;
    public GameObject ShopMachinegun;
    public GameObject ShopMissile;
    public GameObject ShopAirDefense;
    public GameObject ShopMortar;

    // Real Towers
    public GameObject Rifle;
    public GameObject Sniper;
    public GameObject Machinegun;
    public GameObject Missile;
    public GameObject AirDefense;
    public GameObject Mortar;

    // Info for Shop
    TowerController RifleInfo;
    TowerController SniperInfo;
    TowerController MachinegunInfo;
    TowerController MissileInfo;
    TowerController AirDefenseInfo;
    TowerController MortarInfo;

    public TMP_Text CostText;
    public TMP_Text DamageText;
    public TMP_Text RangeText;
    public TMP_Text AccuracyText;
    public TMP_Text FireRateText;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        PlayerInfo = Player.GetComponent<PlayerController>();

        WaveControl = GameObject.Find("WaveController");
        wave = WaveControl.GetComponent<WaveController>();

        Controls = Settings.GetComponent<SettingsController>();

        MenuSettings = GameObject.Find("Canvas").GetComponent<Menu>();

        RifleInfo = Rifle.GetComponent<TowerController>();
        SniperInfo = Sniper.GetComponent<TowerController>();
        MachinegunInfo = Machinegun.GetComponent<TowerController>();
        MissileInfo = Missile.GetComponent<TowerController>();
        AirDefenseInfo = AirDefense.GetComponent<TowerController>();
        MortarInfo = Mortar.GetComponent<TowerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(Controls.keys["Cancel"]))
            {
                if (TempTower != null)
                {
                    Destroy(TempTower);
                    TowerId = 0;
                }
                PlayerInfo.MouseUsed = false;
            }

            if (TempTower != null)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    TempTower.transform.position = hit.point;
                }
            }

            if (Input.GetMouseButtonDown(0) && TempTower != null)
            {
                if (PlayerInfo.scrap >= TempTowerCost)
                {
                    if(CheckTowerDistance())
                    {
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                        {
                            int walkableMask = 1 << NavMesh.GetAreaFromName("Walk");
                            int priorityMask = 1 << NavMesh.GetAreaFromName("Priority");
                            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navmeshHit, 1f, walkableMask) && hit.collider.gameObject.tag != "Enemy")
                            {
                                PlaceTower(hit.point, hit.transform.rotation);
                                PlayerInfo.scrap -= TempTowerCost;
                            }
                            else if (NavMesh.SamplePosition(hit.point, out NavMeshHit navmeshHit2, 1f, priorityMask) && hit.collider.gameObject.tag != "Enemy")
                            {
                                PlaceTower(hit.point, hit.transform.rotation);
                                PlayerInfo.scrap -= TempTowerCost;
                            }
                            else
                            {
                                ErrorMenu.GetComponentInChildren<TMP_Text>().text = "Invalid Location";
                                StartCoroutine(ErrorMessage());
                            }
                        }
                    }
                }
                else
                {
                    ErrorMenu.GetComponentInChildren<TMP_Text>().text = "Not enough scrap.";
                    StartCoroutine(ErrorMessage());
                }
            }

            if (TempTower != null)
            {
                PlayerInfo.MouseUsed = true;
            }

            if (Input.GetKeyDown(Controls.keys["ToggleLines"]))
            {
                lineVisible = !lineVisible;
                foreach(GameObject i in PlacedTowers)
                {
                    i.GetComponent<TowerController>().line.enabled = lineVisible;
                }
            }
        }
    }

    public void SpawnShop(int id)
    {
        // Destroy any old tower
        if (TempTower != null)
        {
            Destroy(TempTower);
        }

        // Set new id
        TowerId = id;

        // Disable Pausing
        MenuSettings.CanPause = false;

        // Spawn corresponding tower
        switch (TowerId)
        {
            case 1:
                TempTower = Instantiate(ShopRifle);
                TempTowerCost = RiflePrice;
                break;

            case 2:
                TempTower = Instantiate(ShopSniper);
                TempTowerCost = SniperPrice;
                break;

            case 3:
                TempTower = Instantiate(ShopMachinegun);
                TempTowerCost = MachinegunPrice;
                break;

            case 4:
                TempTower = Instantiate(ShopMissile);
                TempTowerCost = MissilePrice;
                break;

            case 5:
                TempTower = Instantiate(ShopAirDefense);
                TempTowerCost = AirDefensePrice;
                break;

            case 6:
                TempTower = Instantiate(ShopMortar);
                TempTowerCost = MortarPrice;
                break;

            default:
                break;
        }
    }

    void PlaceTower(Vector3 Point, Quaternion Rot)
    {
        GameObject Temp = null;
        switch (TowerId)
        {
            case 1:
                Temp = Instantiate(Rifle, Point, Rot);
                break;

            case 2:
                Temp = Instantiate(Sniper, Point, Rot);
                break;

            case 3:
                Temp = Instantiate(Machinegun, Point, Rot);
                break;

            case 4:
                Temp = Instantiate(Missile, Point, Rot);
                break;

            case 5:
                Temp = Instantiate(AirDefense, Point, Rot);
                break;

            case 6:
                Temp = Instantiate(Mortar, Point, Rot);
                break;

            default:
                break;
        }

        if (Temp != null)
        {
            PlacedTowers.Add(Temp);
        }

        TowerId = 0;
        Destroy(TempTower);
        TempTower = null;
        PlayerInfo.MouseUsed = false;
        MenuSettings.CanPause = true;
    }

    public bool CheckTowerDistance()
    {
        if (wave.inWave && Vector3.Distance(Player.transform.position, TempTower.transform.position) > PlayerDistance)
        {
            ErrorMenu.GetComponentInChildren<TMP_Text>().text = "To far from player.";
            StartCoroutine(ErrorMessage());
            return false;
        }

        foreach (GameObject i in PlacedTowers)
        {
            if (Vector3.Distance(TempTower.transform.position, i.transform.position) < PlaceDistance)
            {
                ErrorMenu.GetComponentInChildren<TMP_Text>().text = "To close to another tower.";
                StartCoroutine(ErrorMessage());
                return false;
            }
        }
        return true;
    }

    public void SetShopText(int id)
    {
        switch (id)
        {
            case 1:
                CostText.text = "" + RiflePrice;
                DamageText.text = "" + RifleInfo.damage;
                RangeText.text = "" + RifleInfo.range;
                AccuracyText.text = "" + RifleInfo.accuracy;
                FireRateText.text = "" + RifleInfo.fireRate;
                break;

            case 2:
                CostText.text = "" + SniperPrice;
                DamageText.text = "" + SniperInfo.damage;
                RangeText.text = "" + SniperInfo.range;
                AccuracyText.text = "" + SniperInfo.accuracy;
                FireRateText.text = "" + SniperInfo.fireRate;
                break;

            case 3:
                CostText.text = "" + MachinegunPrice;
                DamageText.text = "" + MachinegunInfo.damage;
                RangeText.text = "" + MachinegunInfo.range;
                AccuracyText.text = "" + MachinegunInfo.accuracy;
                FireRateText.text = "" + MachinegunInfo.fireRate;
                break;

            case 4:
                CostText.text = "" + MissilePrice;
                DamageText.text = "" + MissileInfo.damage;
                RangeText.text = "" + MissileInfo.range;
                AccuracyText.text = "" + MissileInfo.accuracy;
                FireRateText.text = "" + MissileInfo.fireRate;
                break;

            case 5:
                CostText.text = "" + AirDefensePrice;
                DamageText.text = "" + AirDefenseInfo.damage;
                RangeText.text = "" + AirDefenseInfo.range;
                AccuracyText.text = "" + AirDefenseInfo.accuracy;
                FireRateText.text = "" + AirDefenseInfo.fireRate;
                break;

            case 6:
                CostText.text = "" + MortarPrice;
                DamageText.text = "" + MortarInfo.damage;
                RangeText.text = "" + MortarInfo.range;
                AccuracyText.text = "" + MortarInfo.accuracy;
                FireRateText.text = "" + MortarInfo.fireRate;
                break;

            default:
                break;
        }
    }

    IEnumerator ErrorMessage()
    {
        ErrorMenu.SetActive(true);
        yield return new WaitForSeconds(2f);
        ErrorMenu.SetActive(false);
    }
}
