using Cysharp.Threading.Tasks;
using IdxZero.Services.Ads;
using IdxZero.Services.Attribution;
using IdxZero.Services.Firebase;

namespace IdxZero.Services.ServicesStarter
{
    public class ServicesStarter : IServicesStarter
    {
        private readonly FirebaseInitializer _firebaseInitializer;
        private readonly IAttributionService _attributionService;
        private readonly IAdsInitializer _adsInitializer;
        private readonly PurchasingServiceInitializer _purchasingInitializer;
        private readonly AdsUtilsStarter _adsUtilsStarter;

        public ServicesStarter(FirebaseInitializer firebaseInitializer,
                               IAttributionService attributionService,
                               IAdsInitializer adsInitializer,
                               PurchasingServiceInitializer purchasingInitializer,
                               AdsUtilsStarter adsUtilsStarter)
        {
            _firebaseInitializer = firebaseInitializer;
            _attributionService = attributionService;
            _adsInitializer = adsInitializer;
            _purchasingInitializer = purchasingInitializer;
            _adsUtilsStarter = adsUtilsStarter;
        }

        public async UniTask StartServices()
        {
            await UniTask.WhenAll(FirebaseInitializationAsync(),
                                  _purchasingInitializer.InitializePurchasingAsync(),
                                  _adsInitializer.InitAdsAsync());

            await UniTask.Yield();
            StartAttributionService();
            _adsUtilsStarter.StartAdsUtils();
        }

        private void StartAttributionService()
        {
            _attributionService.StartAttributionService();
        }

        private UniTask FirebaseInitializationAsync()
        {
            var tcs = new UniTaskCompletionSource<bool>();
            _firebaseInitializer.InitializeFirebase(() =>
            {
                var res = true;
                tcs.TrySetResult(res);
            });
            return tcs.Task;
        }
    }
}