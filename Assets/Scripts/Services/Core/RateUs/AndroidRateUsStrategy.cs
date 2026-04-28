
#if UNITY_ANDROID
using Cysharp.Threading.Tasks;
using Google.Play.Review;

namespace IdxZero.Services.RateUs
{
    public class AndroidRateUsStrategy : IRateUsStrategy
    {
        public UniTask RateUsByStrategy()
        {
            // string rateUrl = "http://play.google.com/store/apps/details?id=" + UnityEngine.Application.identifier;
            // UnityEngine.Application.OpenURL(rateUrl);

            var reviewManager = new ReviewManager();

            // start preloading the review prompt in the background
            var playReviewInfoAsyncOperation = reviewManager.RequestReviewFlow();

            var tcs = new UniTaskCompletionSource<bool>();
            // define a callback after the preloading is done
            playReviewInfoAsyncOperation.Completed += async playReviewInfoAsync =>
            {
                if (playReviewInfoAsync.Error == ReviewErrorCode.NoError)
                {
                    // display the review prompt
                    var playReviewInfo = playReviewInfoAsync.GetResult();
                    await reviewManager.LaunchReviewFlow(playReviewInfo);
                    var res = true;
                    tcs.TrySetResult(res);
                }
                else
                {
                    var res = true;
                    tcs.TrySetResult(res);
                }
            };
            return tcs.Task;
        }
    }
}
#endif