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
	    private const string _defaultTargetName = "Unity-iPhone";

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

		    var projPath = PBXProject.GetPBXProjectPath(path);
		    var xcodeProject = new PBXProject();
		    xcodeProject.ReadFromFile(projPath);

		    Debug.Log(string.Format("PBX Project path: {0}", (object)projPath));
#if UNITY_2019_3_OR_NEWER
			var targetGuid = xcodeProject.GetUnityMainTargetGuid();
#else
		    var targetGuid = xcodeProject.TargetGuidByName(_defaultTargetName);
#endif

		    Debug.Log(string.Format("Target GUID: {0}", targetGuid));
		    xcodeProject.AddFrameworkToProject(targetGuid, "AppTrackingTransparency.framework", true);
		    xcodeProject.WriteToFile(projPath);
#endif
        }
    }
}

