using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mage_Prototype.AbilityLibrary
{
    public class DisplayMesh : AbilityComponent
    {
        private MeshRenderer _renderer;
        private float _duration;
        private float _timer;

        public override void Init(Ability owner, JToken dataFile, int index)
        {
            _renderer = GetComponent<MeshRenderer>();
            _renderer.enabled = false;

            _duration = dataFile[index]["Duration"].Value<float>();

            if (NextComponent != null)
                NextComponent.Init(owner, dataFile, ++index);
        }

        public override void Activate(Character target)
        {
            if (target == null)
                transform.position = Owner.Caster.transform.position;
            else
                transform.position = target.transform.position;

            _renderer.enabled = true;
            _timer = _duration;

            if (NextComponent != null)
                NextComponent.Activate(target);
        }

        public override void Deactivate()
        {
            _renderer.enabled = false;
            base.Deactivate();
        }

        public void Update()
        {
            if (_timer <= 0)
                Deactivate();

            _timer -= Time.deltaTime;
        }
    }
}
