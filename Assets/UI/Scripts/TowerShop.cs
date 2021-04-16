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
    bool valid = false;

    public float PlaceDistance = 6f;
    public float PlayerDistance = 15f;

    public List<GameObject> PlacedTowers = new List<GameObject>();

    PlayerController PlayerInfo;

    public bool lineVisible = false;

    // Temp Towers
    public GameObject TempTower;
    int TempTowerCost = 0;
    int TowerId = 0;
    float TempRange = 0;
    LineRenderer line;
    public Material ValidMat;
    public Material InvalidMat;

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

    AudioSource Speaker;
    public AudioClip SelectTowerSound;
    public AudioClip BuyTowerSound;

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();

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
                PlayerInfo.MouseUsed = true;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    TempTower.transform.position = hit.point;
                    valid = false;

                    // Determine Line color for valid placement
                    if (CheckTowerDistance())
                    {
                        int walkableMask = 1 << NavMesh.GetAreaFromName("Walk");
                        int priorityMask = 1 << NavMesh.GetAreaFromName("Priority");
                        if (NavMesh.SamplePosition(hit.point, out NavMeshHit navmeshHit, 1f, walkableMask) && hit.collider.gameObject.tag != "Enemy")
                        {
                            line.material = ValidMat;
                            valid = true;
                        }
                        else if (NavMesh.SamplePosition(hit.point, out NavMeshHit h2, 1f, priorityMask) && hit.collider.gameObject.tag != "Enemy")
                        {
                            line.material = ValidMat;
                            valid = true;
                        }
                        else
                        {
                            line.material = InvalidMat;
                            valid = false;
                            ErrorMenu.GetComponentInChildren<TMP_Text>().text = "Invalid Location";
                        }
                    }
                    else
                    {
                        line.material = InvalidMat;
                        valid = false;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (valid)
                        {
                            if (PlayerInfo.scrap >= TempTowerCost)
                            {
                                PlaceTower(hit.point, hit.transform.rotation);
                                PlayerInfo.scrap -= TempTowerCost;
                            }
                            else
                            {
                                ErrorMenu.GetComponentInChildren<TMP_Text>().text = "Not enough scrap.";
                                StartCoroutine(ErrorMessage());
                            }
                        }
                        else
                        {
                            StartCoroutine(ErrorMessage());
                        }
                    }
                }
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
        PlayerInfo.placingTower = true;

        Speaker.clip = SelectTowerSound;
        Speaker.PlayOneShot(Speaker.clip);

        // Spawn corresponding tower
        switch (TowerId)
        {
            case 1:
                TempTower = Instantiate(ShopRifle);
                TempTowerCost = RifleInfo.cost;
                TempRange = RifleInfo.range;
                break;

            case 2:
                TempTower = Instantiate(ShopSniper);
                TempTowerCost = SniperInfo.cost;
                TempRange = SniperInfo.range;
                break;

            case 3:
                TempTower = Instantiate(ShopMachinegun);
                TempTowerCost = MachinegunInfo.cost;
                TempRange = MachinegunInfo.range;
                break;

            case 4:
                TempTower = Instantiate(ShopMissile);
                TempTowerCost = MissileInfo.cost;
                TempRange = MissileInfo.range;
                break;

            case 5:
                TempTower = Instantiate(ShopAirDefense);
                TempTowerCost = AirDefenseInfo.cost;
                TempRange = AirDefenseInfo.range;
                break;

            case 6:
                TempTower = Instantiate(ShopMortar);
                TempTowerCost = MortarInfo.cost;
                TempRange = MortarInfo.range;
                break;

            default:
                break;
        }

        // Shop Tower Line
        line = TempTower.GetComponent<LineRenderer>();
        line.positionCount = 50 + 1;
        line.useWorldSpace = false;
        line.startWidth = .2f;
        CreatePoints();
        line.enabled = true;
    }

    void PlaceTower(Vector3 Point, Quaternion Rot)
    {
        GameObject Temp = null;

        Speaker.clip = BuyTowerSound;
        Speaker.PlayOneShot(Speaker.clip);

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
            return false;
        }

        foreach (GameObject i in PlacedTowers)
        {
            if (Vector3.Distance(TempTower.transform.position, i.transform.position) < PlaceDistance)
            {
                ErrorMenu.GetComponentInChildren<TMP_Text>().text = "To close to another tower.";
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
                CostText.text = "" + RifleInfo.cost;
                DamageText.text = "" + RifleInfo.damage;
                RangeText.text = "" + RifleInfo.range;
                AccuracyText.text = "" + RifleInfo.accuracy;
                FireRateText.text = "" + RifleInfo.fireRate;
                break;

            case 2:
                CostText.text = "" + SniperInfo.cost;
                DamageText.text = "" + SniperInfo.damage;
                RangeText.text = "" + SniperInfo.range;
                AccuracyText.text = "" + SniperInfo.accuracy;
                FireRateText.text = "" + SniperInfo.fireRate;
                break;

            case 3:
                CostText.text = "" + MachinegunInfo.cost;
                DamageText.text = "" + MachinegunInfo.damage;
                RangeText.text = "" + MachinegunInfo.range;
                AccuracyText.text = "" + MachinegunInfo.accuracy;
                FireRateText.text = "" + MachinegunInfo.fireRate;
                break;

            case 4:
                CostText.text = "" + MissileInfo.cost;
                DamageText.text = "" + MissileInfo.damage;
                RangeText.text = "" + MissileInfo.range;
                AccuracyText.text = "" + MissileInfo.accuracy;
                FireRateText.text = "" + MissileInfo.fireRate;
                break;

            case 5:
                CostText.text = "" + AirDefenseInfo.cost;
                DamageText.text = "" + AirDefenseInfo.damage;
                RangeText.text = "" + AirDefenseInfo.range;
                AccuracyText.text = "" + AirDefenseInfo.accuracy;
                FireRateText.text = "" + AirDefenseInfo.fireRate;
                break;

            case 6:
                CostText.text = "" + MortarInfo.cost;
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

    void CreatePoints()
    {
        float x;
        float z;

        float angle = 20f;

        if (TempTower.transform.lossyScale.x < 1)
        {
            for (int i = 0; i < (50 + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * TempRange * 4;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * TempRange * 4;

                line.SetPosition(i, new Vector3(x, 0.5f, z));

                angle += (360f / 50);
            }
        }
        else
        {
            for (int i = 0; i < (50 + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * TempRange;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * TempRange;

                line.SetPosition(i, new Vector3(x, 0.5f, z));

                angle += (360f / 50);
            }
        }
    }
}
