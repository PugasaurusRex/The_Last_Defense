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

    AudioSource Speaker;
    public AudioClip ApplySound;
    public AudioClip CancelSound;
    public AudioClip ForwardSound;
    public AudioClip BackwardSound;

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();

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
                Speaker.clip = ApplySound;
                Speaker.PlayOneShot(Speaker.clip);

                ToggleShop();
            }

            // Toggle Inventory
            if (Input.GetKeyDown(Controls.keys["ToggleInventory"]))
            {
                Speaker.clip = ApplySound;
                Speaker.PlayOneShot(Speaker.clip);

                ToggleInv();
            }

            // Pause
            if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(Controls.keys["Cancel"]))
            {
                if (CanPause)
                {
                    Speaker.clip = ApplySound;
                    Speaker.PlayOneShot(Speaker.clip);

                    PauseMenu.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    Speaker.clip = CancelSound;
                    Speaker.PlayOneShot(Speaker.clip);

                    CanPause = true;
                    GameObject.Find("Player").GetComponent<PlayerController>().MouseUsed = false;
                }
            }
        }
    }

    public void setPanel(int p)
    {
        Speaker.clip = ForwardSound;
        Speaker.PlayOneShot(Speaker.clip);

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
        Time.timeScale = 0;
        UI.SetActive(false);
        ShopMenu.SetActive(false);
        GameOverScreen.SetActive(true);
    }

    public void Victory()
    {
        Time.timeScale = 0;
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
            ShopMenu.transform.position = ShopMenu.transform.position + new Vector3(0, -5000, 0);
            ShopToggle = false;
        }
        else
        {
            ShopMenu.transform.position = ShopMenu.transform.position + new Vector3(0, 5000, 0);
            ShopToggle = true;
        }
    }

    public void ToggleInv()
    {
        if (InvToggle)
        {
            Inventory.transform.position = Inventory.transform.position + new Vector3(0, -5000, 0);
            InvToggle = false;
        }
        else
        {
            Inventory.transform.position = Inventory.transform.position + new Vector3(0, 5000, 0);
            InvToggle = true;
        }
    }

    public void ResumeGame()
    {
        Speaker.clip = ForwardSound;
        Speaker.PlayOneShot(Speaker.clip);

        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ToggleSettings(bool visible)
    {
        Speaker.clip = ForwardSound;
        Speaker.PlayOneShot(Speaker.clip);

        SettingsMenu.SetActive(visible);
    }

    public void ToggleExit(bool visible)
    {
        Speaker.clip = BackwardSound;
        Speaker.PlayOneShot(Speaker.clip);

        ExitMenu.SetActive(visible);
    }

    public void ExitToMenu()
    {
        try
        {
            Speaker.clip = BackwardSound;
            Speaker.PlayOneShot(Speaker.clip);

            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        catch
        {
            Application.Quit();
        }
    }

    public void Restart()
    {
        Speaker.clip = ApplySound;
        Speaker.PlayOneShot(Speaker.clip);
        Time.timeScale = 1;
        int temp = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(temp);
    }

    public void NextLevel()
    {
        try
        {
            Speaker.clip = ApplySound;
            Speaker.PlayOneShot(Speaker.clip);

            int temp = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            Time.timeScale = 1;

            if (temp >= 3)
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
