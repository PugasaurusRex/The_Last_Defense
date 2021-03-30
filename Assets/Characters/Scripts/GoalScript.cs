using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public int health = 100;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Player.GetComponent<PlayerController>().Gameover();
            Destroy(this.gameObject);
        }
    }
}
