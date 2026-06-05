using TMPro;
using System.Collections;
using UnityEngine;

public class TextFadeIN : MonoBehaviour
{

    [Header("Continue Text")]
    public TMP_Text continueText;
    public float flashTimeMultiplier = 1f;
    public float continueTextDelay = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ContinueText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ContinueText()
    {
        continueText.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(continueTextDelay);

        float timer = 0f;
        float newAlpha = 0;
        bool paused = false;
        while (newAlpha < .9f)
        {
            newAlpha = Mathf.Sin(timer * flashTimeMultiplier);
            continueText.color = new Color(0, 0, 0, newAlpha);
            timer += Time.deltaTime;
            yield return null;
        }

        while (true)
        {
            newAlpha = Mathf.Abs(Mathf.Sin(timer * flashTimeMultiplier));
            continueText.color = new Color(0, 0, 0, newAlpha);
            if (!paused && newAlpha >= .95f)
            {
                paused = true;
                yield return new WaitForSeconds(continueTextDelay);
            }
            else if (paused && newAlpha < .2f)
            {
                paused = false;
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
