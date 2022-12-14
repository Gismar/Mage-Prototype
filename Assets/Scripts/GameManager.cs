using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Mage_Prototype.AbilityLibrary;
using Mage_Prototype.Effects;
using Cinemachine;

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
            JObject file = (JObject)JToken.Parse(player.ScriptableData.StatsFile.text);

            Dictionary<Trait, int> traits = file["Traits"].Children<JObject>().ToDictionary(
                    key => Enum.Parse<Trait>(key.Properties().First().Name),
                    value => (int)value.Properties().First().Value
            );

            player.Init((string)file["Name"], 1, traits);
            for (int i = 0; i < player.ScriptableData.AbilitiesFiles.Length; i++)
            {
                CreateAbility(
                    character: player,
                    prefab: player.ScriptableData.AbilityPrefabs[i],
                    abilityText: player.ScriptableData.AbilitiesFiles[i].text,
                    level: player.ScriptableData.AbilityLevels[i]
                );
            }

            file = (JObject)JToken.Parse(TrainingDummy.ScriptableData.StatsFile.text);

            traits = file["Traits"].Children<JObject>().ToDictionary(
                    key => Enum.Parse<Trait>(key.Properties().First().Name),
                    value => (int)value.Properties().First().Value
            );

            TrainingDummy.Init((string)file["Name"], 1, traits);
        }

        private static void CreateAbility(Character character, GameObject prefab, string abilityText, int level)
        {
            //JObject file = (JObject)JToken.Parse(abilityText);

            //string name = (string)file["Name"];
            //JToken data = file["Data"].First(d => (int)d["Level"] == level);

            if (InstantAbilityFactory.Instance.TryCreateAbility(prefab, out InstantAbility ability))
            {
                ability.Init(character);
                character.AddAbility(ability);
            }
        }
    }
}
