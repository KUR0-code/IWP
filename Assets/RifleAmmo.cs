using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class RifleAmmo : MonoBehaviour  
{
    public bool HasInteracted = false;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
        count += Random.Range(5, 15);
    }

    // Update is called once per frame
    void Update()
    {
        if(HasInteracted)
        {

        }
    }
}
