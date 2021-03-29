using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    Rigidbody Rig;
    Animator Anim;
    public GameObject ControlMenu;
    SettingsController Controls;
    public GameObject MuzzleFlash;
    public GameObject bulletSpawn;

    // Gamestate Variables
    public int health = 100;
    public int armor = 0;
    public int scrap = 250;

    // Player Variables
    public float maxSpeed = 9.0f;
    public float accel = .8f;

    // Gun Vars
    bool CanShoot = true;
    bool Shooting = false;
    bool Reloading = false;
    public float RateOfFire = 4f;
    public float ReloadTime = 4f;
    public GameObject bullet;

    public int magSize = 13;
    int mag;

    // UI
    public TMP_Text magText;
    public TMP_Text magSizeText;
    public TMP_Text scrapText;
    public TMP_Text healthText;

    public GameObject UI;
    public GameObject GameOver;

    bool MouseUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        Rig = this.GetComponent<Rigidbody>();
        Anim = this.GetComponent<Animator>();
        Controls = ControlMenu.GetComponent<SettingsController>();
    }

    // Update is called once per frame
    void Update()
    {
        //scrapText.text = "" + scrap;
        //healthText.text = "" + health;

        // Get User Input
        float h = 0;
        float v = 0;

        if (Input.GetKey(Controls.keys["Up"]))
        {
            v += 1;
        }
        if (Input.GetKey(Controls.keys["Down"]))
        {
            v += -1;
        }
        if (Input.GetKey(Controls.keys["Right"]))
        {
            h += 1;
        }
        if (Input.GetKey(Controls.keys["Left"]))
        {
            h += -1;
        }

        // Player Character looks at mouse position
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            this.transform.LookAt(new Vector3(hit.point.x, this.transform.position.y, hit.point.z));
        }

        // Accelerate to desired speed
        Vector3 desiredV = new Vector3(h, 0, v).normalized * maxSpeed;
        if ((desiredV - Rig.velocity).magnitude < .1f)
        {
            Rig.velocity = desiredV;
        }
        else
        {
            Rig.velocity += (desiredV - Rig.velocity).normalized * accel;
        }
    }

    public void MouseOnButton()
    {
        MouseUsed = true;
    }

    public void MouseOffButton()
    {
        MouseUsed = false;
    }

    public void TakeDamage(int incomingDamage)
    {
        if (armor >= incomingDamage)
        {
            armor -= incomingDamage;
        }
        else if (armor > 0)
        {
            incomingDamage -= armor;
            armor = 0;
            health -= incomingDamage;
        }
        else
        {
            health -= incomingDamage;
        }
    }
}
