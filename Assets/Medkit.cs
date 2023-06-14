using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public bool BoxInteracted = false;
    public Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator.speed = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        if(BoxInteracted)
        {
            Debug.Log("open");
            // animator.SetTrigger("Open");
            animator.speed = 0.5f;
        }
    }
}
