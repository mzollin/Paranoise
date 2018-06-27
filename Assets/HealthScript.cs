using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    public Slider healthSlider;
    public Image hurtTint;
    public int currentHealth;
    public int maxHealth;
    public bool damageTaken;

    private Color red = new Color(1f, 0f, 0f, .2f);
    private Color death = new Color(1f, 0f, 0f, 1f);

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
        healthSlider.value = currentHealth;
	}
	
	// Update is called once per frame
	void Update () {
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

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        damageTaken = true;
        if(currentHealth < 0)
        {
            hurtTint.color = death;
        }
    }
}
