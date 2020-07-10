#if UNITY_IPHONE && !UNITY_EDITOR
namespace RocketUtils.Models
{
	public class UserKeychain
	{
		public string userId;
		public string uuid;
	}
}
#endif