using Cysharp.Threading.Tasks;

namespace IdxZero.Services.Ads
{
    public interface IAdsInitializer
    {
        UniTask InitAdsAsync();
    }
}

