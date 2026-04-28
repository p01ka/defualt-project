using Cysharp.Threading.Tasks;

namespace IdxZero.Services.RateUs
{
    public interface IRateUsFacade
    {
        UniTask RateUs();
        bool IsPending();
    }
}