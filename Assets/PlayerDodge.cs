using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerDodge : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public bool dodge;
    float disappearTime;
    // Start is called before the first frame update
    void Start()
    {
        dodge = false;
        disappearTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(dodge)
        {
            disappearTime += Time.deltaTime;
            if(disappearTime <= 0.5)
            {
                Debug.Log("here");
                textDisplay.text = "Dodge!";
                dodge = false;  
            }
            FadeOut();
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCR());
    }

    private IEnumerator FadeOutCR()
    {
        float duration = 0.5f; //0.5 secs
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
