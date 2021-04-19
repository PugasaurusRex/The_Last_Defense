using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Components
    Rigidbody Rig;
    public Animator Anim;
    public GameObject ControlMenu;
    public SettingsController Controls;
    public GameObject GunHand;

    public bool placingTower = false;

    // Gamestate Variables
    public int health = 100;
    public int armor = 0;
    public int scrap = 250;

    // Player Variables
    public float maxSpeed = 9.0f;
    public float accel = .8f;
    public float stimSpeed = 0;

    // Active Item
    public GameObject ActiveWeapon;
    public Item ActiveWeaponInfo;

    // UI
    public TMP_Text magText;
    public TMP_Text magSizeText;
    public TMP_Text scrapText;
    public Image healthDisplay;
    public Image armorDisplay;

    public bool MouseUsed = false;

    // Audio
    AudioSource Speaker;
    public AudioClip DieSound;
    public AudioClip WalkSound;
    public AudioClip TakeDamageSound;

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();
        Speaker.volume = PlayerPrefs.GetFloat("volume", 1);

        Rig = this.GetComponent<Rigidbody>();
        Anim = this.GetComponent<Animator>();
        Controls = ControlMenu.GetComponent<SettingsController>();

        ActiveWeaponInfo = ActiveWeapon.GetComponent<Item>();
        ActiveWeaponInfo.mag = ActiveWeaponInfo.magSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            // Set UI Elements
            scrapText.text = "" + scrap;
            magSizeText.text = "" + ActiveWeaponInfo.magSize;
            magText.text = "" + ActiveWeaponInfo.mag;
            healthDisplay.fillAmount = (float)health / 100;
            armorDisplay.fillAmount = (float)armor / 100;

            // Get User Input
            float h = 0;
            float v = 0;

            // Input for moving
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
            Vector3 desiredV = new Vector3(h, 0, v).normalized * (maxSpeed + stimSpeed);
            if ((desiredV - Rig.velocity).magnitude < .1f)
            {
                Rig.velocity = desiredV;
            }
            else
            {
                Rig.velocity += (desiredV - Rig.velocity).normalized * accel;
            }

            // Use Item
            if (Input.GetKeyDown(Controls.keys["Shoot"]) || (Input.GetButtonDown("Fire1") && !MouseUsed && !placingTower))
            {
                ActiveWeaponInfo.usingItem = true;
            }
            else if(Input.GetButtonDown("Fire1") && placingTower)
            {
                placingTower = false;
            }
            else if(Input.GetKeyUp(Controls.keys["Shoot"]) || (Input.GetButtonUp("Fire1")))
            {
                Anim.SetBool("Shoot", false);
                ActiveWeaponInfo.usingItem = false;
            }

            // Reload
            if ((Input.GetKey(Controls.keys["Reload"]) && !ActiveWeaponInfo.reloading) && ActiveWeaponInfo.mag != ActiveWeaponInfo.magSize)
            {
                ActiveWeaponInfo.canUse = false;
                ActiveWeaponInfo.reloading = true;
                ActiveWeaponInfo.StartCoroutine(ActiveWeaponInfo.Reload());
            }

            // If out of health initiate gameover sequence
            if (health <= 0)
            {
                Anim.SetBool("Dead", true);
                GameObject.Find("Canvas").GetComponent<Menu>().Gameover();
            }
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
        try
        {
            Speaker.clip = TakeDamageSound;
            Speaker.PlayOneShot(Speaker.clip);
        }
        catch
        {
            Debug.Log("No Audio for taking damage");
        }

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
