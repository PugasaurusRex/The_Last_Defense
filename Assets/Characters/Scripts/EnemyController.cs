using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    GameObject Goal;
    NavMeshAgent myNav = null;
    bool attacking = false;
    GameObject Player;
    PlayerController PlayerInfo;
    Animator Anim;

    public GameObject Gate;
    public bool passGate = true;
    public bool avoidStun = false;

    GameObject TargetObject;
    public Vector3 target;

    public float attackDistance = 1;
    public float attackCooldown = 1;
    public int damage = 1;
    public int droppedScrap = 10;

    public bool AttackPlayer = false;
    public bool AttackTowers = false;
    public float ViewDistance = 10f;
    public bool flying = false;

    TowerShop TowerShopRef;
    GameObject TowerToTarget;
    bool targeting = false;

    // Enemy Gamestate Variables
    public int health = 100;
    public int armor = 0;

    int maxHealth;
    public Image healthDisplay;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;

        // Get GameObjects
        Player = GameObject.Find("Player");
        PlayerInfo = Player.GetComponent<PlayerController>();

        TowerShopRef = GameObject.Find("TowerShopUI").GetComponent<TowerShop>();
        Goal = GameObject.Find("Goal");

        // Get Animator
        Anim = this.GetComponent<Animator>();

        // Set starting target
        SetTargetGoal();

        // Start navmesh
        myNav = this.gameObject.GetComponent<NavMeshAgent>();
        myNav.destination = target;
        myNav.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            healthDisplay.fillAmount = (float)health / maxHealth;

            healthDisplay.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

            if (!Anim.GetBool("Dead"))
            {
                if (AttackPlayer)
                {
                    if (Vector3.Distance(this.transform.position, Player.transform.position) < ViewDistance)
                    {
                        target = Player.transform.position;
                        TargetObject = Player;
                    }
                    else
                    {
                        SetTargetGoal();
                    }
                }

                if (AttackTowers)
                {
                    if (!targeting)
                    {
                        foreach (GameObject i in TowerShopRef.PlacedTowers)
                        {
                            if (i != null)
                            {
                                if (Vector3.Distance(this.transform.position, i.transform.position) < ViewDistance)
                                {
                                    TargetObject = i;
                                    target = i.transform.position;
                                    targeting = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TargetObject == null)
                        {
                            targeting = false;
                            SetTargetGoal();
                        }
                    }
                }

                this.transform.LookAt(myNav.nextPosition);

                if (Vector3.Distance(this.transform.position, target) < attackDistance && TargetObject != Gate)
                {
                    myNav.isStopped = true;
                    Anim.SetBool("Moving", false);

                    if (!attacking)
                    {
                        attacking = true;
                        Anim.SetBool("Attack", true);
                        StartCoroutine(Attack());
                    }
                }
                else if(TargetObject == Gate && Vector3.Distance(this.transform.position, target) < attackDistance)
                {
                    passGate = true;
                    SetTargetGoal();
                }

                if (health <= 0)
                {
                    myNav.isStopped = true;
                    myNav.velocity = Vector3.zero;

                    this.gameObject.GetComponent<Collider>().enabled = false;

                    Anim.SetBool("Dead", true);
                    GameObject.Find("WaveController").GetComponent<WaveController>().AliveEnemies.Remove(this.gameObject);
                    StartCoroutine(Die());
                }

                if (!attacking && myNav.isOnNavMesh)
                {
                    Anim.SetBool("Moving", true);
                    myNav.destination = target;
                    myNav.isStopped = false;
                }
            }
            else
            {
                myNav.velocity = Vector3.zero;
            }
        }
    }

    IEnumerator Attack()
    {
        StartCoroutine(AttackAnimation());
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
        Anim.SetBool("Attack", false);
    }

    IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(.3f);

        // Deal Damage
        try
        {
            if (TargetObject != null)
            {
                if (AttackPlayer && TargetObject == Player)
                {
                    PlayerInfo.TakeDamage(damage);
                }
                else if (TargetObject == Goal)
                {
                    Goal.GetComponent<GoalScript>().TakeDamage(damage);
                }
                else if (AttackTowers)
                {
                    TargetObject.GetComponent<TowerController>().TakeDamage(damage);
                }
            }
        }
        catch
        {
            TargetObject = null;
            targeting = false;
            SetTargetGoal();
        }
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

    IEnumerator Die()
    {
        yield return new WaitForSeconds(Anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(this.gameObject);
        Player.GetComponent<PlayerController>().scrap += droppedScrap;
    }

    public IEnumerator Stun(float stunTime)
    {
        if(!avoidStun)
        {
            float temp = myNav.speed;
            myNav.speed = 1;
            yield return new WaitForSeconds(stunTime);
            myNav.speed = temp;
        }
        else
        {
            yield return new WaitForSeconds(.1f);
        }
    }

    public void SetTargetGoal()
    {
        if(passGate)
        {
            target = Goal.transform.position;
            TargetObject = Goal;
        }
        else
        {
            target = Gate.transform.position;
            TargetObject = Gate;
        }
    }
}
