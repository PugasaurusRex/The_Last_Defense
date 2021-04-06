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
    public GameObject GameOverScreen;
    public GameObject ShopMenu;
    public GameObject Inventory;
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public GameObject ExitMenu;
    public GameObject VictoryScreen;
    public GameObject WaveInfo;
    public GameObject ShopInfo;

    bool ShopToggle = true;
    bool InvToggle = true;
    public bool CanPause = true;
    int level;

    // Start is called before the first frame update
    void Start()
    {
        Controls = ControlMenu.GetComponent<SettingsController>();
        level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if(level != 0)
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
                    Time.timeScale = 0;
                }
                else
                {
                    CanPause = true;
                }
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
        ShopMenu.SetActive(false);
        GameOverScreen.SetActive(true);
    }

    public void Victory()
    {
        UI.SetActive(false);
        ShopMenu.SetActive(false);
        VictoryScreen.SetActive(true);
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
        Time.timeScale = 1;
    }

    public void ToggleSettings(bool visible)
    {
        SettingsMenu.SetActive(visible);
    }

    public void ToggleExit(bool visible)
    {
        ExitMenu.SetActive(visible);
    }

    public void ExitToMenu()
    {
        try
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        catch
        {
            Application.Quit();
        }
    }

    public void Restart()
    {
        int temp = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(temp);
    }

    public void NextLevel()
    {
        try
        {
            int temp = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            if(temp >= 3)
            {
                ExitToMenu();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(temp + 1);
            }
        }
        catch
        {
            ExitToMenu();
        }
    }

    public void ToggleShopInfo(bool visible)
    {
         ShopInfo.SetActive(visible);
    }
}
