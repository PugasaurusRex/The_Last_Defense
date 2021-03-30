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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        //Anim.SetBool("Attack", false);
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
