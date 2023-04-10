using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    
    public Slider healthBarSlider;
    private float maxHealth = 100;

    public void SetMaxHealth(float maxH){
        maxHealth = maxH;
    }

    public bool SetHealth(float currHealth){
        if(currHealth > maxHealth || currHealth < 0){
            return false;
        }
        healthBarSlider.value = currHealth;
        return true;
    }
}
