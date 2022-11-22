using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Mage_Prototype
{
    public enum Modifier
    {
        Base,
        Bonus,
        Multiplicative,
        Limiter
    }

    public class TraitInfo
    {
        private int _base;
        private int _bonus;
        private int _multiplicative;
        private int _limiter;

        public int this[Modifier type]
        {
            get => type switch
            {
                Modifier.Base => _base,
                Modifier.Bonus => _bonus,
                Modifier.Multiplicative => _multiplicative,
                Modifier.Limiter => _limiter
            };

            private set
            {
                switch (type)
                {
                    case Modifier.Base: _base = value; break;
                    case Modifier.Bonus: _bonus = value; break;
                    case Modifier.Multiplicative: _multiplicative = value; break;
                    case Modifier.Limiter: _limiter = value; break;
                }
            }
        }

        public TraitInfo(int @base, int bonus = 0, int multiplicative = 100, int limiter = -1)
        {
            _base = @base;
            _bonus = bonus;
            _multiplicative = multiplicative;
            _limiter = limiter;
        }

        public void ChangeValueBy(Modifier type, int value) => this[type] += value;

        public float GetTotal()
        {
            float total = (_base * (_multiplicative / 100f)) + _bonus;

            if (_limiter == -1)
                return total;

            return MathF.Min(total, _limiter);
        }
    }
}
