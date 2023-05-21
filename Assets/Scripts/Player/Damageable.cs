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
    // Start is called before the first frame update
    void Awake()
    {
        CurrentHp = maxHp;
    }

    public void takeDamage(float damage, Vector3 HitPos, Vector3 HitNormal)
    {
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
            died = false;
            // stun lock 
        }
    }

    public void Die()
    {
        print(name + " was destroyed");
        Animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        died = true;

    }
}
