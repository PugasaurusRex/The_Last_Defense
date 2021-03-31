using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public int health = 100;
    public GameObject MenuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            MenuUI.GetComponent<Menu>().Gameover();
            Destroy(this.gameObject);
        }
    }
}
