using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelScript : MonoBehaviour {
    private SaveInfoObject obj;
    void Start()
    {
        obj = SaveInfo.saveInfo.SaveObject;
    }
    public void IncreaseFrequency()
    {
        obj.AttackFrequency /= 1.5f;
    }
    public void IncreaseHealth()
    {
        obj.MaxHealth += 200;
    }   
    public void IncreaseDamage()
    {
        obj.MainAttackStrength += 5;
    }
}
