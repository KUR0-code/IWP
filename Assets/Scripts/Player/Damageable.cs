using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    public float maxHp = 100f;
    public float CurrentHp;
    [SerializeField]
    GameObject hitEffect;
    public bool died = false;
    [SerializeField]
    private Animator Animator;
    float StunTimer;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentHp = maxHp;
        died = false;
        StunTimer = 0;
    }

    private void Update()
    {
        StunTimer += Time.deltaTime;
    }

    public void takeDamage(float damage, Vector3 HitPos, Vector3 HitNormal)
    {
        Instantiate(hitEffect, HitPos, Quaternion.LookRotation(HitNormal));
        CurrentHp -= damage;
        if (CurrentHp >= 0)
        {
            if (StunTimer >= 5)
            {
                Animator.SetTrigger("Damage");
                StunTimer = 0;
            }
        }
    }

    public float RegendHealth(float damage)
    {
        CurrentHp += damage;
        return CurrentHp;
    }
    public void Die()
    {
        print(name + " was destroyed");
        Animator.SetTrigger("Die");
        died = true;
    }
}
