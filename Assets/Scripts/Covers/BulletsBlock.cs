using Character;
using UnityEngine;

namespace Covers
{
    public class BulletsBlock : MonoBehaviour, IDamageTaker
    {
        public void TakeDamage(int damage) { }

        public Faction Faction => Faction.None;
    }
}