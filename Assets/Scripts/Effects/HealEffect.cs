using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="CardGame/Effects/IntegerEffect/Heal Effect",fileName ="Heal Effect",order =5)]

public class HealEffect : IntegerEffect, IEntityEffect
{
    public override string GetName()
    {
        return $"Heal {Value.ToString()} HP";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        var targetHp = target.Hp;
        var finalHp = targetHp.Value + Value;

        if( finalHp > target.MaxHp)
        {
            finalHp = target.MaxHp;
            Debug.Log("this");
        }

        targetHp.SetValue(finalHp);
        Debug.Log("Heal");
    }
}
