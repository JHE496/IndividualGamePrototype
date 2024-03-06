using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectResolutionManager : BaseManager
{
    public CardSelectionHasArrow cardSelectionHasArrow;
    private CharacterObject _currentEnemy;

    public void ResolveCardEffects(RuntimeCard card, CharacterObject playerSelectedTarget)
    {
        foreach (var effect in card.Template.Effects)
        {
            var targetableEffect = effect as TargetableEffect;

            if (targetableEffect != null)
            {
                var targets = GetTargets(targetableEffect, playerSelectedTarget, true);
                foreach (var target in targets)
                {
                    targetableEffect.Resolve(Player.Character, target.Character);

                    foreach (var groupManager in targetableEffect.SourceActions)
                    {
                        foreach (var action in groupManager.Group.Actions)
                        {
                            action.Execute(Player.gameObject);
                        }
                    }

                    foreach (var groupManager in targetableEffect.TargetActions)
                    {
                        foreach (var action in groupManager.Group.Actions)
                        {
                            var enemy = cardSelectionHasArrow.GetSelectedEnemy();
                            action.Execute(enemy.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void ResolveCardEffects(RuntimeCard card)
    {
        foreach (var effect in card.Template.Effects)
        {
            var targetableEffect = effect as TargetableEffect;

            if (targetableEffect != null)
            {
                var targets = GetTargets(targetableEffect, playerSelectedTarget:null, playerSource:true);

                foreach (var target in targets)
                {
                    targetableEffect.Resolve(Player.Character,target.Character);
                }
            }
        }
    }


    private List<CharacterObject> GetTargets(
        TargetableEffect effect,
        CharacterObject playerSelectedTarget,
        bool playerSource)
    {
        var targets = new List<CharacterObject>(4);
        
        // 如果动作发起方是主角Player
        if (playerSource)
        {
            switch (effect.Target)
            {
                case EffectTargetType.Self:
                    targets.Add(Player);
                    break;
                case EffectTargetType.TargetEnemy:
                    targets.Add(playerSelectedTarget);
                    break;
            }
        }
        // 如果动作发起方是敌人
        else
        {
            switch (effect.Target)
            {
                case EffectTargetType.Self:
                    targets.Add(_currentEnemy);
                    break;
                case EffectTargetType.TargetEnemy:
                    targets.Add(Player);
                    break;
            }
        }

        return targets;
    }
}
