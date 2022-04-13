using RocketUtils.Models;

namespace RocketUtils.Extensions
{
    public static class AdFormatExtensions
    {
        public static AdFormat ToAdFormat(this string adFormat)
        {
            if (adFormat.Contains("banner"))
                return AdFormat.Banner;

            if (adFormat.Contains("interstitial"))
                return AdFormat.Interstitial;

            if (adFormat.Contains("rewarded"))
                return AdFormat.RewardedVideo;
            
            return AdFormat.None;
        }
    }
}
