using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    public int level;
    public float currentXP;
    public float requiredXP;
    private float lerpTimer;
    private float delayTimer;

    [Header("UI")]
    public Image frontXpBar;
    public Image backXpBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI XpText;

    [Header("Multiplier")]
    [Range(1f,300)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 2;
    [SerializeField]
    GameObject Enemy;
    // Start is called before the first frame update
    void Awake()
    {
        frontXpBar.fillAmount = currentXP / requiredXP;
        backXpBar.fillAmount = currentXP / requiredXP;
        requiredXP = RequiredXp();
        levelText.text = "Level " + level;
        Enemy = GameObject.FindGameObjectWithTag("Dragon");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateXpUI();

        if (Enemy.GetComponent<Damageable>().died)
        {
            Enemy.GetComponent<Damageable>().died = false;
            //Debug.Log("xp gained");
        }
        if (currentXP > requiredXP)
        {
            LevelUp();
        }
    }

    public void UpdateXpUI()
    {
        float xpFraction = currentXP / requiredXP;
        float FrontXp = frontXpBar.fillAmount;
        if(FrontXp < xpFraction)
        {
            delayTimer += Time.deltaTime;
            backXpBar.fillAmount = xpFraction;
            if(delayTimer > 1)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4;
                // frontXpBar.fillAmount = Mathf.Lerp(FrontXp, backXpBar.fillAmount, percentComplete);
            }
        }
        XpText.text = currentXP + "/" + requiredXP;
    }

    public void GainExpRate(float xpGained)
    {
        currentXP += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;
    }
    public void ExperienceScale(float xpGained, int passedLevel)
    {
        if(passedLevel < level)
        {
            float multiplier = 1 + (level - passedLevel) * 0.1f;
            currentXP += xpGained * multiplier;
        }
        else
        {
            currentXP += xpGained;
        }

        lerpTimer = 0f;
        delayTimer = 0f;
    }
    public void LevelUp()
    {
        level++;
        frontXpBar.fillAmount = 0f;
        backXpBar.fillAmount = 0f;
        currentXP = Mathf.RoundToInt(currentXP - requiredXP);
        GetComponent<PlayerHealth>().IncreaseHealth(level);
        requiredXP = RequiredXp();
        levelText.text = "Level " + level;
    }

    private int RequiredXp()
    {
        int requiredXp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            requiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }

        return requiredXp / 4;
    }
}
