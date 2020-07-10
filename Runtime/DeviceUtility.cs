#if UNITY_IOS && !UNITY_EDITOR
using RocketUtils.Models;
using RocketUtils.Plugins.iOS.Keychain;
#endif

using UnityEngine;

namespace RocketUtils
{
	public static class DeviceUtility
	{
		public static string GetDeviceOs()
		{
			string deviceOs = SystemInfo.operatingSystem;

#if UNITY_STANDALONE && !UNITY_EDITOR
			deviceOs = "Standalone";
#elif UNITY_EDITOR
			deviceOs = "UnityEditor";
#endif
			return deviceOs;
		}

		public static string GetDeviceModel()
		{
			string deviceModel = SystemInfo.deviceModel;

#if UNITY_STANDALONE && !UNITY_EDITOR
			deviceModel = "Standalone";
#elif UNITY_EDITOR
			deviceModel = "UnityEditor";
#endif
			return deviceModel;
		}

		public static string GetDeviceUniqueId()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
			AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
			return secure.CallStatic<string>("getString", contentResolver, "android_id");
#elif UNITY_IPHONE && !UNITY_EDITOR
			//get device id from keychain
			//if it doesn't exist save new deviceid to keychain
			var keychain = KeyChain.BindGetKeyChainUser();
			var userKeychain = JsonUtility.FromJson<UserKeychain>(keychain);
			if (!string.IsNullOrEmpty(userKeychain.uuid))
	        {
				return userKeychain.uuid;
			}
			KeyChain.BindSetKeyChainUser("0", SystemInfo.deviceUniqueIdentifier);
			return SystemInfo.deviceUniqueIdentifier;
#else
			return SystemInfo.deviceUniqueIdentifier;
#endif
		}
	}
}
