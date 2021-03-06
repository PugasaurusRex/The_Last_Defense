using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Actions a;
    // Start is called before the first frame update
    void Start()
    {
        a = this.GetComponent<Actions>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Vertical") > 0)
        {
            a.Aiming();
        }
    }
}
