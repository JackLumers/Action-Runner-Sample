using Globals;
using UnityEngine;

namespace ReferenceVariables
{
    [CreateAssetMenu(
        fileName = "New " + nameof(IntVariable),
        menuName = ProjectConstants.ScriptableObjectsAssetMenuName +
                   "/" + ProjectConstants.VariablesAssetMenuName +
                   "/Create new " + nameof(IntVariable))]
    public class IntVariable : ScriptableObject
    {
        [SerializeField] private int _value;

        public int Value
        {
            get => _value;
            set => _value = value;
        }
    }
}