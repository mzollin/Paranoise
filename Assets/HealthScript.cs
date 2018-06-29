using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    public Slider healthSlider;
    public Image hurtTint;
    public GameObject gameOver;
    public int currentHealth;
    public int maxHealth;
    public bool damageTaken;

    private Color red = new Color(1f, 0f, 0f, .2f);
    private Color death = new Color(1f, 0f, 0f, 1f);

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
        healthSlider.value = currentHealth;
        //gameOver.color = Color.clear;
        //gameOver.enabled = false;
        gameOver.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (currentHealth > 0)
        {
            if (damageTaken)
            {
                hurtTint.color = red;
                damageTaken = false;
            }
            else
            {
                hurtTint.color = Color.Lerp(hurtTint.color, Color.clear, 5 * Time.deltaTime);
            }
        }
	}

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        damageTaken = true;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            hurtTint.color = death;
            //gameOver.color = Color.black;
            //gameOver.enabled = true;
            gameOver.GetComponentInChildren<Text>().text = Mathf.Round(Time.time * 100f)/100f + "s";
            gameOver.SetActive(true);
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthSlider.value = currentHealth;
    }
}
