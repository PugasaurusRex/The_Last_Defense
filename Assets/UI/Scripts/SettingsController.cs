using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    // Keycode Dictionary
    public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    private GameObject currentKey;
    private Color32 normal = new Color(255, 255, 255, 255);
    private Color32 selected = new Color(255, 0, 0, 255);
    
    // Controls Textbox
    public TMP_Text up;
    public TMP_Text down;
    public TMP_Text left;
    public TMP_Text right;
    public TMP_Text shoot;
    public TMP_Text reload;
    public TMP_Text cancel;
    public TMP_Text shop;
    public TMP_Text inventory;
    public TMP_Text swap;
    public TMP_Text lines;
    public TMP_Text map;

    // Start is called before the first frame update
    void Start()
    {
        // Add Keys to dictionary
        keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keys.Add("Shoot", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Shoot", "Space")));
        keys.Add("Reload", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Reload", "R")));
        keys.Add("Cancel", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Cancel", "Escape")));
        keys.Add("ToggleShop", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToggleShop", "E")));
        keys.Add("ToggleInventory", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToggleInventory", "F")));
        keys.Add("SwapWeapon", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("SwapWeapon", "Q")));
        keys.Add("ToggleLines", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToggleLines", "T")));
        keys.Add("ToggleCamera", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToggleCamera", "C")));

        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(currentKey != null)
        {
            Event e = Event.current;
            if(e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<TMP_Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        if(currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
        }

        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }

    public void SaveKeys()
    {
        foreach(var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }

        PlayerPrefs.Save();
    }

    public void Back()
    {
        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
            currentKey = null;
        }
        SetText();

    }

    public void SetText()
    {
        up.text = PlayerPrefs.GetString("Up", "W");
        down.text = PlayerPrefs.GetString("Down", "S");
        left.text = PlayerPrefs.GetString("Left", "A");
        right.text = PlayerPrefs.GetString("Right", "D");
        shoot.text = PlayerPrefs.GetString("Shoot", "Space");
        reload.text = PlayerPrefs.GetString("Reload", "R");
        cancel.text = PlayerPrefs.GetString("Cancel", "Escape");
        shop.text = PlayerPrefs.GetString("ToggleShop", "E");
        inventory.text = PlayerPrefs.GetString("ToggleInventory", "F");
        swap.text = PlayerPrefs.GetString("SwapWeapon", "Q");
        lines.text = PlayerPrefs.GetString("ToggleLines", "T");
        map.text = PlayerPrefs.GetString("ToggleMap", "C");
    }
}
