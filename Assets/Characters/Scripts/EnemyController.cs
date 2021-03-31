using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    GameObject Goal;
    NavMeshAgent myNav = null;
    bool attacking = false;
    GameObject Player;
    PlayerController PlayerInfo;
    Animator Anim;

    GameObject TargetObject;
    public Vector3 target;

    public float attackDistance = 1;
    public float attackCooldown = 1;
    public int damage = 1;
    public int droppedScrap = 10;

    public bool AttackPlayer = false;
    public bool AttackTowers = false;
    public float ViewDistance = 10f;

    TowerShop TowerShopRef;
    GameObject TowerToTarget;
    bool targeting = false;

    // Enemy Gamestate Variables
    public int health = 100;
    public int armor = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get GameObjects
        Player = GameObject.Find("Player");
        PlayerInfo = Player.GetComponent<PlayerController>();

        TowerShopRef = GameObject.Find("TowerShopUI").GetComponent<TowerShop>();
        Goal = GameObject.Find("Goal");

        // Get Animator
        Anim = this.GetComponent<Animator>();

        // Set starting target
        target = Goal.transform.position;
        TargetObject = Goal;

        // Start navmesh
        myNav = this.gameObject.GetComponent<NavMeshAgent>();
        myNav.destination = target;
        myNav.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
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
                    target = Goal.transform.position;
                    TargetObject = Goal;
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
                        target = Goal.transform.position;
                        TargetObject = Goal;
                    }
                }
            }

            this.transform.LookAt(myNav.nextPosition);

            if (Vector3.Distance(this.transform.position, target) < attackDistance)
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

            if (health <= 0)
            {
                myNav.isStopped = true;
                myNav.velocity = Vector3.zero;

                this.gameObject.GetComponent<Collider>().enabled = false;

                Anim.SetBool("Dead", true);
                GameObject.Find("SpawnerControl").GetComponent<WaveController>().AliveEnemies.Remove(this.gameObject);
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

    IEnumerator Attack()
    {
        Anim.SetBool("Attack", false);

        if (AttackPlayer && TargetObject == Player)
        {
            PlayerInfo.TakeDamage(damage);
        }
        else if (TargetObject == Goal)
        {
            GoalScript temp = Goal.GetComponent<GoalScript>();
            temp.health -= damage;
        }
        else
        {
            TargetObject.GetComponent<TowerController>().TakeDamage(damage);
        }
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
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
        yield return new WaitForSeconds(Anim.runtimeAnimatorController.animationClips[3].length);
        Destroy(this.gameObject);
        Player.GetComponent<PlayerController>().scrap += droppedScrap;
    }
}
