using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;

    public GameObject ControlMenu;
    SettingsController Controls;

    public GameObject UI;
    public GameObject GameOver;
    public GameObject ShopMenu;
    public GameObject Inventory;
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public GameObject ExitMenu;
    public GameObject VictoryScreen;
    public GameObject WaveInfo;

    bool ShopToggle = true;
    bool InvToggle = true;
    public bool CanPause = true;

    // Start is called before the first frame update
    void Start()
    {
        Controls = ControlMenu.GetComponent<SettingsController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle Shop
        if (Input.GetKeyDown(Controls.keys["ToggleShop"]))
        {
            ToggleShop();
        }

        // Toggle Inventory
        if (Input.GetKeyDown(Controls.keys["ToggleInventory"]))
        {
            ToggleInv();
        }

        // Pause
        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(Controls.keys["Cancel"]))
        {
            if (CanPause)
            {
                PauseMenu.SetActive(true);
            }
            else
            {
                CanPause = true;
            }
        }
    }

    public void setPanel(int p)
    {
        switch (p)
        {
            case 1:
                p1.SetActive(true);
                p2.SetActive(false);
                p3.SetActive(false);
                break;
            case 2:
                p1.SetActive(false);
                p2.SetActive(true);
                p3.SetActive(false);
                break;
            case 3:
                p1.SetActive(false);
                p2.SetActive(false);
                p3.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Gameover()
    {
        UI.SetActive(false);
        GameOver.SetActive(true);
    }

    public void Victory()
    {

    }

    public void ToggleShop()
    {
        if (ShopToggle)
        {
            if (ShopMenu.GetComponent<TowerShop>().TempTower != null)
            {
                Destroy(ShopMenu.GetComponent<TowerShop>().TempTower);
                ShopMenu.GetComponent<TowerShop>().TempTower = null;
            }
            ShopMenu.SetActive(false);
            ShopToggle = false;
        }
        else
        {
            ShopMenu.SetActive(true);
            ShopToggle = true;
        }
    }

    public void ToggleInv()
    {
        if (InvToggle)
        {
            Inventory.SetActive(false);
            InvToggle = false;
        }
        else
        {
            Inventory.SetActive(true);
            InvToggle = true;
        }
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
    }

    public void ToggleSettings(bool visible)
    {
        if (visible)
        {
            SettingsMenu.SetActive(true);
        }
        else
        {
            SettingsMenu.SetActive(false);
        }
    }

    public void ToggleExit(bool visible)
    {
        if (visible)
        {
            ExitMenu.SetActive(true);
        }
        else
        {
            ExitMenu.SetActive(false);
        }
    }
}
