﻿using System.Collections.Generic;
using Code.Scripts.PassiveAbilities.Abstraction;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using UnityEngine;

namespace Code.Scripts.PassiveAbilities.AbilitiesSO
{
    [CreateAssetMenu(fileName = "New HealthIncrease", menuName = "PassiveAbilities/HealthIncreaseAbility")]
    public class HealthIncreaseSO : PassiveAbilityBase
    {
        [SerializeField] private List<float> healthIncreasePercentages;

        public override void ApplyEffect(StatsManipulator statsManipulator)
        {
            
            if (currentLevel >= 5)
                return;
            float oldMaxHealth = statsManipulator.maxHealth;
            float healthBonus = statsManipulator.MaxHealth * (healthIncreasePercentages[currentLevel - 1] / 100f);
            statsManipulator.maxHealth = statsManipulator.MaxHealth+ healthBonus;
            float healthRatio = statsManipulator.maxHealth / oldMaxHealth;
            statsManipulator.currentHealth *= healthRatio;
        }

        public override void RemoveEffect(StatsManipulator statsManipulator)
        {
            if (currentLevel > 0)
            {
                // float healthBonus = statsManipulator.maxHealth * (healthIncreasePercentages[currentLevel - 1] / 100f);
                // statsManipulator.maxHealth -= healthBonus;
                // statsManipulator.currentHealth = statsManipulator.maxHealth;
                currentLevel = 0;
            }
        }
    }
}
