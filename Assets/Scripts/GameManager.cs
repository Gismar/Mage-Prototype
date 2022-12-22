using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Mage_Prototype.AbilityLibrary;
using Mage_Prototype.Effects;
using Cinemachine;
using System.Data;

namespace Mage_Prototype
{
    public class GameManager : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public Character TrainingDummy;
        public CinemachineVirtualCamera Camera;

        public void Start()
        {
            Character player = Instantiate(PlayerPrefab).GetComponent<Character>();
            Camera.Follow = player.transform;
            JObject file = JObject.Parse(player.ScriptableData.StatsFile.text);

            Dictionary<Trait, int> traits = file["Traits"].Children<JObject>().ToDictionary(
                    key => Enum.Parse<Trait>(key.Properties().First().Name),
                    value => (int)value.Properties().First().Value
            );

            player.Init((string)file["Name"], 1, traits);
            foreach (AbilityScriptable ability in player.ScriptableData.Abilities)
                CreateAbility(player, ability);

            file = (JObject)JToken.Parse(TrainingDummy.ScriptableData.StatsFile.text);

            traits = file["Traits"].Children<JObject>().ToDictionary(
                    key => Enum.Parse<Trait>(key.Properties().First().Name),
                    value => (int)value.Properties().First().Value
            );

            TrainingDummy.Init((string)file["Name"], 1, traits);
        }

        private static void CreateAbility(Character character, AbilityScriptable scriptableData)
        {
            JObject file = JObject.Parse(scriptableData.BaseDataFile.text);

            string name = file["Name"].Value<string>();
            JToken data = file["LevelData"][scriptableData.Level.ToString()];

            foreach (ModifierScriptable modScriptable in scriptableData.Modifiers)
            {
                JObject modFile = JObject.Parse(modScriptable.DataFile.text);
                JToken modData = modFile["LevelData"][modScriptable.Level.ToString()];
                MergeModifier(data, modData);
            }

            if (InstantAbilityFactory.Instance.TryCreateAbility(scriptableData.AbilityPrefab, out InstantAbility ability))
            {
                ability.Init(character, data, name);
                character.AddAbility(ability);
            }
        }

        private static void MergeModifier(JToken data, JToken modifier)
        {
            int modifyingCount = modifier.Children().Count();

            for (int i = 0; i < modifyingCount; i++)
            {
                JObject item = data.First(p => p["Name"].Value<string>() == modifier[i]["Name"].Value<string>()) as JObject;
                item.Merge((JObject)modifier[i]);
            }
        }
    }
}
