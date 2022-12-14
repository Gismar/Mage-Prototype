using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Mage_Prototype.AbilityLibrary;
using static UnityEngine.Rendering.DebugUI;
using System.Buffers;

namespace Mage_Prototype
{
    /// <summary>
    /// Will be converted to ECS 
    /// </summary>
    public class DamageDisplayComponent : MonoBehaviour
    {
        [SerializeField] private DamageDisplay _damageDisplayPrefab;

        private Dictionary<Element, Color> _colors = new()
        {
            [Element.True]          = new Color(1.00f, 1.00f, 1.00f), // #FFFFFF
            [Element.Physical]      = new Color(0.92f, 0.59f, 0.00f), // #EB9600
            [Element.Projectile]    = new Color(0.36f, 0.62f, 0.39f), // #5D9D63
            [Element.Magical]       = new Color(0.69f, 0.32f, 0.62f), // #AF519F

            [Element.Fire]          = new Color(0.86f, 0.27f, 0.13f), // #DC4422
            [Element.Ice]           = new Color(0.43f, 0.61f, 0.76f), // #6D9BC3
            [Element.Light]         = new Color(0.79f, 0.79f, 0.51f), // #FAFAD2
            [Element.Dark]          = new Color(0.60f, 0.60f, 0.60f), // #7A7A7A
        };
        private string[] _tags = new string[] { "<sub>B</sub>", "<sub>M</sub>", "<sub>K</sub>", "" };

        private List<DamageDisplay> _damageDisplayPool;
        private int _maximumDisplays = 15;
        private int _storedDamage;

        public void Awake()
        {
            _damageDisplayPool = new(_maximumDisplays);
        }

        public void Display(int damage, Element element, bool isCrit)
        {
            DamageDisplay damageDisplay = GetCanvasFromPool(out int count);

            if (damageDisplay == null)
            {
                _storedDamage += damage;
                return;
            }

            damageDisplay.transform.position = transform.position + Vector3.up * (3 + count / 2f);
            string parsedDamage = ParseToUnitString(damage, isCrit);

            if (_storedDamage > 0)
            {
                string stored = ParseToUnitString(_storedDamage, false);
                parsedDamage += $"<color=\"white\"> + {stored}";
                _storedDamage = 0;
            }

            damageDisplay.gameObject.SetActive(true);
            damageDisplay.Activate(_colors[element], parsedDamage);
        }

        private string ParseToUnitString(int damage, bool isCrit)
        {
            string[] parsed = damage.ToString("#,###").Split(',');
            string combined = isCrit ? "★" : "";

            if (parsed.Length > 1)
            {
                int offset = _tags.Length - parsed.Length;

                for (int i = 0; i < 2; i++)
                    combined += parsed[i] + _tags[i + offset] + " ";
            }
            else
            {
                combined += parsed[0];
            }
            combined += isCrit ? "★" : "";
            return combined;
        }

        private DamageDisplay GetCanvasFromPool(out int count)
        {
            int temp = 0;
            DamageDisplay damageDisplay = _damageDisplayPool.FirstOrDefault(c => { temp++; return !c.gameObject.activeInHierarchy; });
            count = temp;
            if (count > _maximumDisplays)
                return null;

            if (damageDisplay == default)
            {
                damageDisplay = Instantiate(_damageDisplayPrefab, transform);
                _damageDisplayPool.Add(damageDisplay);
            }
            return damageDisplay;
        }
    }
}
