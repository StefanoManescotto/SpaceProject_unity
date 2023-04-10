using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGuns {
    public void Shoot(Quaternion rotation, string layerName);
    public string GetGunID();
    public void DoAbility(string idAbility, float cooldown);
}
