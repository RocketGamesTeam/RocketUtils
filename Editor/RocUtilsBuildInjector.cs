#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RocketUtils.Editor
{
	public class RocAdBuildInjector
	{
		[PostProcessBuild(1000)]
		public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
		{
#if UNITY_IOS
            if (buildTarget != BuildTarget.iOS) return;

            // Get plist
            string plistPath = string.Format("{0}{1}Info.plist", path, Path.DirectorySeparatorChar);
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));

			// Get root
			PlistElementDict rootDict = plist.root;

		    rootDict.SetString("NSUserTrackingUsageDescription", "This identifier will be used to deliver personalized ads to you.");

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
#endif
        }
    }
}

