using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    public int health = 100;
    public GameObject MenuUI;

    public int maxHealth;
    public Image healthDisplay;

    AudioSource Speaker;
    public AudioClip TakeDamageSound;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        Speaker = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            MenuUI.GetComponent<Menu>().Gameover();
            Destroy(this.gameObject);
        }
        else
        {
            healthDisplay.fillAmount = (float)health / maxHealth;
            healthDisplay.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
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
            Debug.Log("Failed to play damage audio");
        }

        health -= incomingDamage;
    }
}
