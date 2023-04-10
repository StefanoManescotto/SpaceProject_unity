using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastFireAbility : IAbility {
    private string idAbility = "fastFire";
    private float cooldown = 2f;
    public Slider slider;

    override public void ExecuteAbility(List<IGuns> guns) {
        if(canExecute){
            foreach(IGuns g in guns){
                if(g.GetGunID() == "gun1"){
                    g.DoAbility(idAbility, cooldown);
                }
            }
            StartCoroutine(Cooldown(cooldown));
            StartCoroutine(CooldownVisual(cooldown));
        }
    }

    protected IEnumerator CooldownVisual(float cooldown) {
        float divisions = 100;
        slider.value = 1;
        for(int i = 0; i < divisions; i++){
            yield return new WaitForSeconds(cooldown / divisions);
            slider.value -= (1f / divisions);
        }
        
    }

    override public string GetIdAbility(){
        return idAbility;
    }
}
