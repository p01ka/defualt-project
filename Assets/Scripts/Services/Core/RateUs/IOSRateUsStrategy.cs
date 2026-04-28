#if UNITY_IOS
using Cysharp.Threading.Tasks;
using IdxZero.Services.RateUs;
using UnityEngine.iOS;

public class IOSRateUsStrategy : IRateUsStrategy
{
    public async UniTask RateUsByStrategy()
    {
        Device.RequestStoreReview();
        await UniTask.DelayFrame(10);
    }
}
#endif