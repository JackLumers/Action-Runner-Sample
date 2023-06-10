using Globals;
using UnityEngine;

namespace ReferenceVariables
{
    [CreateAssetMenu(
        fileName = "New " + nameof(IntVariable),
        menuName = ProjectConstants.ScriptableObjectsAssetMenuName +
                   "/" + ProjectConstants.VariablesAssetMenuName +
                   "/Create new " + nameof(FloatVariable))]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField] private float _value;

        public float Value
        {
            get => _value;
            set => _value = value;
        }
    }
}