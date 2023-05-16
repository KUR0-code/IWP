using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public GameObject Dragon;
    public Slider HealthBar;

    private void Update()
    {
        HealthBar.value = Dragon.GetComponent<Damageable>().CurrentHp;
        Debug.Log(HealthBar.value);
    }
}
