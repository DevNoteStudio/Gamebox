using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    [RequireComponent(typeof(ContextStartup))]
    public abstract class SceneContext : MonoBehaviour
    {
        private List<Type> _registeredTypes = new();


        public abstract void RegisterContext();

        private void OnDestroy() => UnregisterContext();

 
        protected T Register<T>(T controller) where T : class
        {
            Context.Register(controller);
            _registeredTypes.Add(typeof(T));
            return controller;
        }

        private void UnregisterContext()
        {
            foreach (var type in _registeredTypes)
                Context.Unregister(type);
        }


    }
}


