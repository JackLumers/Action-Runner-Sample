using System.Collections.Generic;
using Globals;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(
        fileName = "New " + nameof(GameEvent),
        menuName = ProjectConstants.ScriptableObjectsAssetMenuName +
                   "/" + ProjectConstants.GameEventsAssetMenuName +
                   "/Create new " + nameof(GameEvent))]
    public class GameEvent : ScriptableObject
    {
        private readonly HashSet<GameEventListener> _listeners = new();

        public void Raise()
        {
            foreach (var listener in _listeners) 
                listener.OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }
    }
}