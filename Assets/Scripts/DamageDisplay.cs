using System.Collections;
using TMPro;
using UnityEngine;

namespace Mage_Prototype
{
    public class DamageDisplay: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Animator _animator;

        public void Activate(Color color, string info) // Plays Animations
        {
            _text.color = color;
            _text.text = info;
            _animator.SetTrigger("Play");
            StartCoroutine(nameof(Deactivate));
        }

        private IEnumerator Deactivate()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}
