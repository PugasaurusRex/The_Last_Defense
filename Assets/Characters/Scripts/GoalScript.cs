using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    public int health = 100;
    public GameObject MenuUI;

    int maxHealth;
    public Image healthDisplay;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
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
}
