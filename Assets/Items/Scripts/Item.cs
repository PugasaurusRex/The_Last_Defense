using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public int cost;
    public float reloadTime;
    public float rateOfFire;
    public bool rifle = false;

    public bool usingItem = false;
    public bool canUse = true;
    public bool reloading = false;

    public int magSize = 13;
    public int mag = 0;

    public GameObject Player;
    public PlayerController PlayerInfo;

    GameObject Hand;
    public Vector3 HandPosition = new Vector3(0.0047f, -0.0969f, 0.0354f);
    public Vector3 HandRotation = new Vector3(-90, -90, -90);

    // Start is called before the first frame update
    public virtual void Start()
    {
        Player = GameObject.Find("Player");
        PlayerInfo = Player.GetComponent<PlayerController>();
        Hand = PlayerInfo.GunHand;

        SetItemActive();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(Time.timeScale != 0)
        {
            // Use if possible
            if (usingItem && canUse && mag > 0)
            {
                canUse = false;
                PlayerInfo.Anim.SetBool("Shoot", true);
                StartCoroutine(Use());
            }
            else if (mag <= 0 && !reloading)
            {
                canUse = false;
                reloading = true;
                StartCoroutine(Reload());
            }
        }
    }

    public void SetItemActive()
    {
        PlayerInfo.Anim.SetBool("Rifle", rifle);

        this.transform.parent = Hand.transform;
        this.transform.localPosition = HandPosition;
        this.transform.localEulerAngles = HandRotation;
    }

    public virtual IEnumerator Use()
    {
        yield return new WaitForSeconds(1);
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        mag = magSize;
        canUse = true;
        reloading = false;
    }
}
