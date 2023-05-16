using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    [Header("Damage Overlay")]
    public Image overlay; // blood splatter
    public float duration; // duration of it appearing
    public float fade; // fades
    private float durationTimer; // checks the duration

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        updateHealthUI();  
        if(overlay.color.a > 0)
        {
            if(health < 30)
                return;
            
            durationTimer += Time.deltaTime;
            if(durationTimer > duration)
            {
                // fade the image
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fade;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void updateHealthUI()
    {
        Debug.Log(health);

        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float HealthFraction = health / maxHealth;

        if(fillBack > HealthFraction)
        {
            frontHealthBar.fillAmount = HealthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, HealthFraction, percentComplete);
        } 

        if(fillFront  < HealthFraction)
        {            
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = HealthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            //percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }

        healthText.text = Mathf.Round(health) + "/" + Mathf.Round(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
    }

    public void IncreaseHealth(int level)
    {
        maxHealth += (health * 0.01f) * ((100 - level) *0.1f);
        health = maxHealth;
    }
}
