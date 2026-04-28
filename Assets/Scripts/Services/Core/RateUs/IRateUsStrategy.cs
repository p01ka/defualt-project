using Cysharp.Threading.Tasks;

namespace IdxZero.Services.RateUs
{
    public interface IRateUsStrategy
    {
        UniTask RateUsByStrategy();
    }
}