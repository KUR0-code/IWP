using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Chest : MonoBehaviour
{

    public bool BoxInteracted = false;
    public Animator animator;
    public int count;
    public bool HasOpened;

    // Start is called before the first frame update
    void Start()
    {
        animator.speed = 0;
        HasOpened = false;
        count += Random.Range(15, 30);
    }

    // Update is called once per frame
    void Update()
    {
        if (BoxInteracted)
        {
            // Debug.Log("open");
            // animator.SetTrigger("Open");
            animator.speed = 0.5f;
            HasOpened = true;
        }
    }
}
