using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdxZero.Services.RateUs
{
    public class EditorRateUsStrategy : IRateUsStrategy
    {
        public async UniTask RateUsByStrategy()
        {
            Debug.Log("EDITOR RATE US!!!!");
            await UniTask.DelayFrame(20);
        }
    }
}