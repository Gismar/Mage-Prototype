using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mage_Prototype
{
    public interface ICharacterComponent
    {
        void Init(Dictionary<Trait, int> baseTraits);
        bool TryGetTraitInfo(Trait trait, out TraitInfo traitInfo);
    }
}
