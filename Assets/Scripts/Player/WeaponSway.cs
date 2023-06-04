using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("swap settings")]
    [SerializeField]
    private float smooth;
    [SerializeField]
    private float swayMultiplier;
  
    
    // Update is called once per frame
    void Update()
    {
        float WeaponX = Input.GetAxisRaw("Mouse X")* swayMultiplier;
        float WeaponY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-WeaponY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(WeaponX, Vector3.up);
        Quaternion targetRotation = rotationX * rotationY;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
