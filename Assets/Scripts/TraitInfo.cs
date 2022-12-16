using System;

namespace Mage_Prototype
{
    public enum Modifier
    {
        Base,
        Bonus,
        Multiplicative,
        Limiter,
        Final
    }

    public sealed class TraitInfo
    {
        private int _base; // Changed by Armor
        private int _multiplicative; // Changed by Armor
        private int _bonus; // Changed by Buffs
        private int _final; // Changed by Buffs
        private int _limiter; // Changed by Debuffs

        public int this[Modifier type]
        {
            get => type switch
            {
                Modifier.Base => _base,
                Modifier.Bonus => _bonus,
                Modifier.Multiplicative => _multiplicative,
                Modifier.Final => _final,
                Modifier.Limiter => _limiter
            };

            private set
            {
                switch (type)
                {
                    case Modifier.Base: _base = value; break;
                    case Modifier.Bonus: _bonus = value; break;
                    case Modifier.Multiplicative: _multiplicative = value; break;
                    case Modifier.Final: _final = value; break;
                    case Modifier.Limiter: _limiter = value; break;
                }
            }
        }

        public TraitInfo(int @base, int bonus = 0, int multiplicative = 100, int limiter = -1, int final = 100)
        {
            _base = @base;
            _bonus = bonus;
            _multiplicative = multiplicative;
            _limiter = limiter;
            _final = final;
        }

        public void ChangeValueBy(Modifier type, int value) => this[type] += value;

        public float GetTotal()
        {
            float total = (_base * (_multiplicative * 0.01f)) + _bonus;
            total *= _final * 0.01f;

            if (_limiter == -1)
                return total;

            return MathF.Min(total, _limiter);
        }
    }
}
