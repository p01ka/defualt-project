using System;
using System.Collections.Generic;
// using Firebase.Extensions;
using UnityEngine;

namespace IdxZero.Services.Firebase
{
    public class FirebaseInitializer
    {
        private readonly List<IFirebaseService> _firebaseServices;
        private readonly int _servicesCount;

        private bool _isFirebaseInitialized = false;
        private int _currentInitializedServicesCount;
        private Action _firebaseInitializedCallback;

        // global::Firebase.DependencyStatus dependencyStatus = global::Firebase.DependencyStatus.UnavailableOther;

        public FirebaseInitializer(List<IFirebaseService> firebaseServices)
        {
            _firebaseServices = firebaseServices;
            _servicesCount = _firebaseServices.Count;
        }

        public void InitializeFirebase(Action firebaseInitializedCallback)
        {
            _firebaseInitializedCallback = firebaseInitializedCallback;
            _firebaseInitializedCallback?.Invoke();
            if (_isFirebaseInitialized) return;
            // global::Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            // {
            //     dependencyStatus = task.Result;
            //     if (dependencyStatus == global::Firebase.DependencyStatus.Available)
            //     {
            //         InitializeFirebaseServices();
            //     }
            //     else
            //     {
            //         Debug.LogError(
            //             "Could not resolve all Firebase dependencies: " + dependencyStatus);
            //     }
            // });
        }

        private void InitializeFirebaseServices()
        {
            foreach (var service in _firebaseServices)
            {
                service.InitializeService(ServiceInitialized);
            }
            _isFirebaseInitialized = true;
        }

        private void ServiceInitialized()
        {
            _currentInitializedServicesCount++;
            if (_currentInitializedServicesCount == _servicesCount)
            {
                _firebaseInitializedCallback?.Invoke();
            }
        }
    }
}
