using Events;
using ReferenceVariables;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ValueDisplay : GameEventListener
    {
        [SerializeField] private IntVariable _variable;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _prefix;

        public void UpdateValue()
        {
            _text.text = $"{_prefix} {_variable.Value}";
        }
    }
}