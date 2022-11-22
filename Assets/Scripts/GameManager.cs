using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Mage_Prototype.Abilities;
using Mage_Prototype.Effects;

namespace Mage_Prototype
{
    public class GameManager : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public Character TrainingDummy;

        public void Awake()
        {
            Character player = Instantiate(PlayerPrefab).GetComponent<Character>();
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
            JObject file = (JObject)JToken.Parse(abilityText);

            string name = (string)file["Name"];
            JToken data = file["Data"].First(d => (int)d["Level"] == level);

            AbilityContainer? ability;
            switch ((string)file["Type"])
            {
                case "AttackAbility":
                    //if (data["CC"] != null)
                    //{
                    //    string ccName = (string)data["CC"]["Name"];
                    //    (bool m, bool a, bool c) = _crowdControls[ccName];
                    //    int dur = (int)data["CC"]["Duration"];
                    //    var ccData = new CrowdControlData(ccName, m, a, c, dur);
                    //    if (EffectFactory<CrowdControl, CrowdControlData>.Instance.TryCreateEffect(ccData, out CrowdControl? ccEffect))
                    //    {
                    //        ability = Helper.CreateAttackAbility(level, name, data, (string)file["Element"], ccEffect);
                    //        if (ability != null) character.AddAbility(ability, name);
                    //        break;
                    //    }
                    //}

                    ability = CreateAttackAbility(prefab, level, name, data, (string)file["Element"]);
                    break;

                case "Buff":
                    ability = CreateBuffAbility(prefab, level, name, data);
                    break;

                case "NonStackingBuff":
                    ability = CreateBuffAbility(prefab, level, name, data, false);
                    break;
                default: 
                    ability = null;
                    break;
            }

            if (ability != null)
            {
                ability.Init(character);
                character.AddAbility(ability);
            }
        }

        public static BuffAbility? CreateBuffAbility(GameObject prefab, int level, string name, JToken json, bool stackable = true)
        {
            List<Effect<BuffData>> data = new();
            foreach (JToken value in json["BuffData"])
            {
                BuffData temp = new() {
                    Trait = Enum.Parse<Trait>((string)value["Attribute"]),
                    Modifier = Enum.Parse<Modifier>((string)value["Modifying"]),
                    Value = (int)value["Value"],
                    Duration = (int)value["Duration"]
                };

                if (stackable)
                {
                    if (EffectFactory<Buff, BuffData>.Instance.TryCreateEffect(temp, out Buff? effect))
                        data.Add(effect);
                }
                else
                {
                    if (EffectFactory<NonStackingBuff, BuffData>.Instance.TryCreateEffect(temp, out NonStackingBuff? effect))
                        data.Add(effect);
                }
            }

            BuffAbilityData abilityData = new() { 
                Name = name, 
                Level = level, 
                Effects = data.ToArray(), 
                Stackable = stackable 
            };

            AbilityFactory<BuffAbility, BuffAbilityData>.Instance.TryCreateAbility(abilityData, prefab, out BuffAbility? ability);
            return ability;
        }

        public static InstantAbility? CreateAttackAbility(GameObject prefab, int level, string name, JToken json, string element)
        {
            Element attackElement = Enum.Parse<Element>(element);

            InstantAbilityData data = new(){
                Name = name, 
                Level = level, 
                Damage = (int)json["Damage"], 
                Element = attackElement, 
                Cost = (Cost.Stamina, 10)
            };
            AbilityFactory<InstantAbility, InstantAbilityData>.Instance.TryCreateAbility(data, prefab, out InstantAbility? ability);

            return ability;
        }

        public static InstantAbility? CreateAttackAbility(GameObject prefab, int level, string name, JToken json, string element, CrowdControl cc)
        {
            Element attackElement = Enum.Parse<Element>(element);
            Action<Character>[] extra = new Action<Character>[] { (c) => cc.Apply(c) };

            InstantAbilityData data = new()
            {
                Name = name,
                Level = level,
                Damage = (int)json["Damage"],
                Element = attackElement,
                Cost = (Cost.Stamina, 10),
                Extras = extra
            };
            AbilityFactory<InstantAbility, InstantAbilityData>.Instance.TryCreateAbility(data, prefab, out InstantAbility? ability);

            return ability;
        }
    }
}
