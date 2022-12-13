using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mage_Prototype
{
    public class HUDBarUpdater : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Slider _advancedHealthBar;
        [SerializeField] private Slider _resourceBar;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ResourceComponent _resourceComponent;

        // Update is called once per frame
        void Update()
        {
            UpdateHealthBar();
            UpdateAdvancedHealthBar();
            UpdateResourceBar();
        }
        private void UpdateHealthBar()
        {
            if (_healthComponent.IsHealthFull)
                _healthBar.value = 1;
            else
                _healthBar.value = (float)_healthComponent.CurrentHealth / _healthComponent.MaxHealth;
        }

        private void UpdateAdvancedHealthBar()
        {
            if (_healthComponent.IsAdvancedHealthEmpty)
                _advancedHealthBar.value = 0;
            else
                _advancedHealthBar.value = _healthComponent.AdvancedHealth / (_healthComponent.MaxHealth * _healthComponent.AdvancedHealth);
        }

        private void UpdateResourceBar()
        {
            if (_resourceComponent.IsResourceFull)
                _resourceBar.value = 1;
            else
                _resourceBar.value = (float)_resourceComponent.CurrentResource / _resourceComponent.MaxResource;
        }
    }
}
