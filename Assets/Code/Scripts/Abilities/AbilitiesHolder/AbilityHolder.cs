using System;
using System.Collections.Generic;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using Code.Scripts.Player;
using UnityEditor;
using UnityEngine;

namespace Code.Scripts.Abilities.AbilitiesHolder
{
    public class AbilityHolder : MonoBehaviour
    {
        [SerializeField] private List<ActiveAbilityBase> abilities;

        private bool canUseAbilities = true;
        
        private void Awake()
        {
            Application.quitting += ResetSkills;
            LevelManager.Instance.OnSceneLoad += ResetSkills;
            GameManager.Instance.OnLooseGame += OnLooseGame;
        }

        private void Start()
        {
            canUseAbilities = true;
            abilities = FindObjectOfType<StatsManipulator>().ActiveAbilities;
        }


        public void ResetSkills()
        {
            foreach (var abilityBase in abilities)
            {
                var ability = abilityBase;
                ability.ResetToDefault();
            }
        }

        private void Update()
        {
            if(!canUseAbilities)
                return;
            
            foreach (var abilityBase in abilities)
            {
                var ability = (ActiveAbilityBase)abilityBase;
                ability.UpdateCooldown();

                PlayerCanvas.Instance.UpdateAbilityCooldown();
            }

            TryActivateAbilities();
        }
        
        private void OnLooseGame()
        {
            canUseAbilities = false;
        }

        private void TryActivateAbilities()
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].hasCooldown && abilities[i].currentCooldown <= 0)
                {
                    abilities[i].Activate(gameObject);
                    abilities[i].currentCooldown = abilities[i].cooldown;
                }
                else if (!abilities[i].hasCooldown && abilities[i].currentCooldown == 0)
                {
                    abilities[i].Activate(gameObject);
                    abilities[i].currentCooldown = -1;
                }
            }
        }
    }
}