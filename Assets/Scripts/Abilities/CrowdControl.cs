using System;
using System.Collections;
using Mage_Prototype.Effects;
using UnityEngine;

namespace Mage_Prototype
{
    public class CrowdControl : Effect<CrowdControlData>
    {
        public override void Apply(Character target)
        {
            _target = target;
            _target.CrowdControl.Add(this);

            StartCoroutine(Remove());
            Debug.Log($"{_target.Name} {Data.Name} for {Data.Duration}s");
        }

        public override IEnumerator Remove()
        {
            yield return new WaitForSeconds(Data.Duration);
            _target.CrowdControl.Remove(this);
        }
    }
}
