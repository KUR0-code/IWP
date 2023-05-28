using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public GameObject Dragon;
    public Slider HealthBar;
    Transform player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        HealthBar.value = Dragon.GetComponent<Damageable>().CurrentHp;
        HealthBar.transform.LookAt(player);
    }
}
