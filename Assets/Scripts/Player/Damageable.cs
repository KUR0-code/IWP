using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    public float maxHp = 100f;
    public float CurrentHp;
    [SerializeField]
    GameObject hitEffect;
    [SerializeField]
    public bool died = false;
    public Animator Animator;

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Awake()
    {
        CurrentHp = maxHp;
        died = false;
    }

    public void takeDamage(float damage, Vector3 HitPos, Vector3 HitNormal)
    {
        // Debug.Log(died);
        Instantiate(hitEffect, HitPos, Quaternion.LookRotation(HitNormal));
        CurrentHp -= damage;
        if(CurrentHp <= 0 )
        {
            //play animation
            Die();
        }
        else
        {
            Animator.SetTrigger("Damage");
            // stun lock 
        }
    }

    public void Die()
    {
        print(name + " was destroyed");
        Animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        died = true;
        //if (gameObject.GetComponent<Collider>().CompareTag("Dragon"))
        //{
        //    // Dragon EXP
        //    player.GetComponent<LevelSystem>().GainExpRate(10);
        //}
        //else if (gameObject.GetComponent<Collider>().CompareTag("Boss"))
        //{
        //    player.GetComponent<LevelSystem>().GainExpRate(50);

        //}

    }
}
