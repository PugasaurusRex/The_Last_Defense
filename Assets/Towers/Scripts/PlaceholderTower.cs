using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderTower : MonoBehaviour
{
    public List<GameObject> Close = new List<GameObject>();
    public bool CanPlace = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Close.Count == 0)
        {
            CanPlace = true;
        }
        else
        {
            CanPlace = false;

            foreach (GameObject i in Close)
            {
                if (i == null)
                {
                    Close.Remove(i);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other);
        if ((other.gameObject.tag == "Tower" || other.gameObject.tag == "Enemy") && !Close.Contains(other.gameObject))
        {
            Close.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Close.Contains(other.gameObject))
        {
            Close.Remove(other.gameObject);
        }
    }
}
