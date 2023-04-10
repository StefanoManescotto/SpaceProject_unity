using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbility : MonoBehaviour {
    protected bool canExecute = true;
    public abstract void ExecuteAbility(List<IGuns> guns);
    public abstract string GetIdAbility();

    protected IEnumerator Cooldown(float cooldown) {
        canExecute = false;
        yield return new WaitForSeconds(cooldown);
        canExecute = true;
    }
}
