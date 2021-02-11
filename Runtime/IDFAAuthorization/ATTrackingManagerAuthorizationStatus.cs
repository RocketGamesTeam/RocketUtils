#if UNITY_IOS

namespace RocketUtils.IDFAAuthorization
{
    public enum ATTrackingManagerAuthorizationStatus
    {
        ATTrackingManagerAuthorizationStatusNotDetermined = 0,
        ATTrackingManagerAuthorizationStatusRestricted,
        ATTrackingManagerAuthorizationStatusDenied,
        ATTrackingManagerAuthorizationStatusAuthorized
    }
}

#endif