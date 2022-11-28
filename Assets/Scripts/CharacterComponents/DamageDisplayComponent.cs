using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Mage_Prototype.Abilities;
using static UnityEngine.Rendering.DebugUI;
using System.Buffers;

namespace Mage_Prototype
{
    public class DamageDisplayComponent : MonoBehaviour
    {
        [SerializeField] private DamageDisplay _damageDisplayPrefab;

        private Dictionary<Element, Color> _colors = new()
        {
            [Element.True]          = new Color(1.00f, 1.00f, 1.00f), // #FFFFFF White
            [Element.Physical]      = new Color(0.92f, 0.59f, 0.00f), // #EB9600 Gamboge
            [Element.Projectile]    = new Color(0.36f, 0.62f, 0.39f), // #5D9D63 Forest Green Crayola
            [Element.Magical]       = new Color(0.31f, 0.32f, 0.50f), // #4E5180 Purple Navy

            [Element.Fire]          = new Color(0.70f, 0.13f, 0.13f), // #B22222 Firebrick
            [Element.Ice]           = new Color(0.43f, 0.61f, 0.76f), // #6D9BC3 Cerulean Frost
            [Element.Light]         = new Color(0.98f, 0.98f, 0.82f), // #FAFAD2 Light Goldenrod Yellow
            [Element.Dark]          = new Color(0.19f, 0.10f, 0.20f), // #301934 Dark Purple
        };
        private string[] _tags = new string[] { "<sub>B</sub>", "<sub>M</sub>", "<sub>K</sub>", "" };

        private List<DamageDisplay> _damageDisplayPool;

        public void Awake()
        {
            _damageDisplayPool = new(10);
        }

        public void Display(int damage, Element element, bool isCrit)
        {
            DamageDisplay damageDisplay = GetCanvasFromPool(out int count);
            damageDisplay.transform.position = transform.position + Vector3.up * (3 + count / 2f);

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

            damageDisplay.gameObject.SetActive(true);
            damageDisplay.Activate(_colors[element], combined);
        }

        private DamageDisplay GetCanvasFromPool(out int count)
        {
            int temp = 0;
            DamageDisplay damageDisplay = _damageDisplayPool.FirstOrDefault(c => { temp++; return !c.gameObject.activeInHierarchy; });
            count = temp;
            if (damageDisplay == default)
            {
                damageDisplay = Instantiate(_damageDisplayPrefab, transform);
                _damageDisplayPool.Add(damageDisplay);
            }
            return damageDisplay;
        }
    }
}
