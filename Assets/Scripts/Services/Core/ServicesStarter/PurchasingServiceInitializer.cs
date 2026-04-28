using Cysharp.Threading.Tasks;

namespace IdxZero.Services.ServicesStarter
{
    public class PurchasingServiceInitializer
    {
        // private readonly IInAppManager _inAppManager;
        // private readonly InAppChecker _inAppChecker;

        // public PurchasingInitializer(IInAppManager inAppManager,InAppChecker inAppChecker)
        // {
        //     _inAppManager = inAppManager;
        //     _inAppChecker = inAppChecker;
        // }

        public UniTask InitializePurchasingAsync()
        {
            return InitializeGameServices().ContinueWith(InitializePurchasingInternalAsync);
        }

        private UniTask InitializeGameServices()
        {
            // try
            // {
            //     var options = new InitializationOptions()
            //         .SetEnvironmentName("production");

            //     return UnityServices.InitializeAsync(options).AsUniTask();
            // }
            // catch (Exception exception)
            // {
            //     UnityEngine.Debug.Log("GAME SERVICES INITIALIZATION ERROR CODE " + exception);
            //     return default;
            // }
            return UniTask.NextFrame();
        }

        private UniTask InitializePurchasingInternalAsync()
        {
            // var tcs = new UniTaskCompletionSource<bool>();
            // _inAppManager.InitializePurchasing(() =>
            // {
            //     _inAppChecker.CheckInApps();
            //     var res = true;
            //     tcs.TrySetResult(res);
            // });
            // return tcs.Task;
            return UniTask.NextFrame();
        }
    }
}