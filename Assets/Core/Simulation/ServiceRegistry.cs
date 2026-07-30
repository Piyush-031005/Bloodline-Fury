using System;
using System.Collections.Generic;

namespace BloodLine.Core
{
    /// <summary>
    /// A lightweight, deterministic Service Registry for dependency injection.
    /// Strictly avoids the Singleton pattern and reflection-based resolution.
    /// Services must be explicitly registered and retrieved via an injected instance of this registry.
    /// </summary>
    public class ServiceRegistry
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a service instance to its type.
        /// </summary>
        public void Register<T>(T service) where T : class
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service), "Cannot register a null service.");
            }

            var type = typeof(T);
            
            if (_services.ContainsKey(type))
            {
                throw new InvalidOperationException($"[ServiceRegistry] A service of type {type.Name} is already registered.");
            }

            _services[type] = service;
        }

        /// <summary>
        /// Retrieves a registered service by type.
        /// Throws an exception if not found, enforcing strict dependency requirements.
        /// </summary>
        public T Get<T>() where T : class
        {
            var type = typeof(T);
            
            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }

            throw new InvalidOperationException($"[ServiceRegistry] Service of type {type.Name} is not registered. Ensure it is registered during bootstrap.");
        }

        /// <summary>
        /// Attempts to retrieve a registered service without throwing an exception.
        /// Useful for optional dependencies.
        /// </summary>
        public bool TryGet<T>(out T service) where T : class
        {
            var type = typeof(T);
            
            if (_services.TryGetValue(type, out var obj))
            {
                service = obj as T;
                return true;
            }

            service = null;
            return false;
        }

        /// <summary>
        /// Unregisters a service. 
        /// Use cautiously, as other systems may hold references to the removed service.
        /// </summary>
        public void Unregister<T>() where T : class
        {
            var type = typeof(T);
            
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
            }
        }

        /// <summary>
        /// Clears all registered services. Useful for testing or full application reboots.
        /// </summary>
        public void Clear()
        {
            _services.Clear();
        }
    }
}
