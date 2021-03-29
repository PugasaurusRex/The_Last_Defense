using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;

    public float zoom = 20f;
    public float zSpeed = 17f;

    public float maxZoom = 70;
    public float minZoom = 20;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxisRaw("Mouse ScrollWheel");

        if (z > 0 && zoom < maxZoom)
        {
            zoom += z * zSpeed;
        }
        if (z < 0 && zoom > minZoom)
        {
            zoom += z * zSpeed;
        }

        this.transform.position = Player.transform.position + new Vector3(0, zoom, 0);
    }
}
