using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShop : MonoBehaviour
{
    public GameObject Player;
    public GameObject WaveControl;
    WaveController wave;
    public float PlaceDistance = 5f;
    public float PlayerDistance = 15f;

    public List<GameObject> PlacedTowers = new List<GameObject>();

    PlayerController PlayerInfo;

    // Tower Prices
    public int RiflePrice = 100;
    public int SniperPrice = 100;
    public int MachinegunPrice = 100;

    // Temp Towers
    public GameObject TempTower;
    int TempTowerCost = 0;
    int TowerId = 0;

    public GameObject ShopRifle;
    public GameObject ShopSniper;
    public GameObject ShopMachinegun;

    // Real Towers
    public GameObject Rifle;
    public GameObject Sniper;
    public GameObject Machinegun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
