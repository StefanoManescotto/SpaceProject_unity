using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGeneralParameters {

    private float maxHealth = 100f;
    private float maxShield = 100f;
    private float health = 100f;
    private float shield = 100f;
    private bool canBeHit = true;
    private bool canRegen = true;
    public HealthBar healthBar;
    public ShieldBar shieldBar;
    private Coroutine regenCountdownCoroutine = null;

    void Start() {
        StartCoroutine(ShieldRegen());
    }

    public void setCanBeHit(bool canBeHit){
        this.canBeHit = canBeHit;
    }

    public float GetHealth() {
        return health;
    }

    private void SetHealth(float newHealth){
        newHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        health = newHealth;
        healthBar.SetHealth(health);
    }

    private void SetShield(float newShield){
        newShield = Mathf.Clamp(newShield, 0, maxShield);
        shield = newShield;
        shieldBar.SetShield(shield);
    }

    public void DealDamage(float damage) {
        if(damage < 0){
            return;
        }

        if(canBeHit){
            if(regenCountdownCoroutine != null){
                StopCoroutine(regenCountdownCoroutine);
            }
            regenCountdownCoroutine = StartCoroutine(ShieldRegenCountDown());
            float oldShield = shield;
            SetShield(shield - damage);
            damage -= oldShield;

            if(damage < 0){
                damage = 0;
            }

            SetHealth(health - damage);

            if(health <= 0){
                //Destroy(gameObject);
            }
        }
    }

    private IEnumerator ShieldRegenCountDown() {
        canRegen = false;
        yield return new WaitForSeconds(2f);
        canRegen = true;
    }

    private IEnumerator ShieldRegen() {
        while(true){
            yield return new WaitForSeconds(.2f);
            if(canRegen){
                SetShield(shield + .2f);
            }
        }
    }
}
