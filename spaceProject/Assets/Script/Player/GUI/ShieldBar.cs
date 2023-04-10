using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour {
    
    public Slider shieldBarSlider;
    private float maxShield = 100;

    public void SetMaxShield(float maxS){
        maxShield = maxS;
    }

    public bool SetShield(float currShield){
        if(currShield > maxShield || currShield < 0){
            return false;
        }
        shieldBarSlider.value = currShield;
        return true;
    }
}
