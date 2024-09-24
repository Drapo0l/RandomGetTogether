using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] float maxHealth;

    public Image healthBar;
    public Image[] healthPoints;

    float health;
    float lerpSpeed;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(health > maxHealth) health = maxHealth;

        lerpSpeed =  3f * Time.deltaTime;

        HealthBarFiller();
        ColorChanger();
    }

   public void HealthBarFiller()
    { 
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health/ maxHealth), lerpSpeed);

        for (int i = 0; i < healthPoints.Length; ++i)
        {
            healthPoints[i].enabled = !DisplayHealthPoint(health, i);
        }
    }
    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        healthBar.color = healthColor;
    }

    bool DisplayHealthPoint(float health, int pointNumber)
    {
        return ((pointNumber * 10) >= health);
    }
    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
        {
            health += healingPoints;
        }
    }
}
