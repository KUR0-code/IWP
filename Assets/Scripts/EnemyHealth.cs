using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public GameObject Dragon;
    public Slider HealthBar;
    Transform player;

    [SerializeField]
    bool isBoss;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        HealthBar.value = Dragon.GetComponent<Damageable>().CurrentHp;
        HealthBar.transform.LookAt(player);

        HasWon();
    }

    private void HasWon()
    {
        
        if (isBoss && HealthBar.value <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
}
