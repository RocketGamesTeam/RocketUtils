using System;
using System.Collections;
using System.Diagnostics;
using RocketCoroutine;
using RocketLog;
using UnityEngine;

namespace RocketUtils
{
	public static class InternetConnectionController
    {
        public static event Action<bool> StatusChanged;
        public static event Action<bool> ConnectionSourceChanged;

		/// <summary>
		/// Ping time out duration in milliseconds
		/// </summary>
		public static int PingTimeOutDuration = 100;

		public static bool HasConnection
		{
			get { return _hasConnection; }
			set
			{
				if (_hasConnection == value) return;
				_hasConnection = value;

				if (StatusChanged != null) StatusChanged(_hasConnection);
			}
		}

		private static bool _hasConnection;

		public static NetworkReachability InternetReachability
		{
			get { return _internetReachability; }
			set
			{
				if (_internetReachability == value) return;

				_internetReachability = value;

				if (ConnectionSourceChanged != null) ConnectionSourceChanged(HasConnection);

				HasConnection = _internetReachability != NetworkReachability.NotReachable;
			}
		}

		private static NetworkReachability _internetReachability;

	    private static bool _isInitialized;

		private static readonly RocLog Log = new RocLog(typeof(InternetConnectionController).Name, DebugLevels.Info);


		public static void Initialize()
		{
			if(_isInitialized) return;

			_isInitialized = true;

			InternetReachability = Application.internetReachability;
			CoroutineController.StartCoroutine(CheckInternetConnectionRoutine());
		}

		public static void DoAfterConnectionAvailable(Action action)
		{
			if (HasConnection)
			{
				action();
			}
			else
			{
				Action<bool> eventHandler = null;
				eventHandler = delegate (bool status)
				{
					if (status)
					{
						action();
						StatusChanged -= eventHandler;
					}
				};

				StatusChanged += eventHandler;
			}
		}

		static IEnumerator CheckInternetConnectionRoutine()
		{
			while (true)
			{
				InternetReachability = Application.internetReachability;
				yield return new WaitForSecondsRealtime(1f);
			}
		}

		/// <summary>
		/// Pings the given url. URL SHOULD BE IN IPv4 FORMAT
		/// </summary>
		/// <param name="url">Should be in IPv4 format</param>
		/// <param name="callback">Returns boolean for success</param>
		public static void PingUrl(string url, Action<bool> callback)
		{
			int count = url.Split('.').Length - 1;
			if (count != 3)
			{
				// Notation is probably wrong.
				Log.Warning("Are you sure you have given the url in IPv4 format?");
			}

			CoroutineController.StartCoroutine(PingRoutine(url, callback), "PingRoutine");
		}

		/// <summary>
		/// Pings 8.8.8.8 and returns a bool callback.
		/// </summary>
		/// <param name="callback"></param>
		public static void PingGoogle(Action<bool> callback)
		{
			CoroutineController.StartCoroutine(PingRoutine("8.8.8.8", callback), "PingRoutine");
		}


		private static IEnumerator PingRoutine(string url, Action<bool> callback)
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();
			Ping ping = new Ping(url);

			Log.Debug("Ping started");

			bool shouldSkip = false;
			CoroutineController.DoAfterGivenTime(PingTimeOutDuration/1000f, () => shouldSkip = true, "PingSkipTimer");

			yield return new WaitUntil(() => ping.isDone || shouldSkip);


			watch.Stop();

			if (ping.isDone)
			{
				Log.Debug("Ping completed.");
				Log.Debug("Ping time: " + ping.time);

				// If pinging exceeds the default time (timeout), Unity returns -1 as ping.time
				// This ping.time < 0 check is for that reason
				if (ping.time < 0 || ping.time > PingTimeOutDuration)
				{
					// ----FAIL----
					Log.Warning("Rateus Timeout. ping.time:" + ping.time);
					callback.Invoke(false);

					if (CoroutineController.IsCoroutineRunning("PingSkipTimer"))
					{
						CoroutineController.StopCoroutine("PingSkipTimer");
					}
				}
				else
				{
					// ----SUCCESS-----
					callback.Invoke(true);
				}
			}
			else if (shouldSkip)
			{
				// ----FAIL----
				Log.Warning("Ping failed.");
				callback.Invoke(false);
			}
			else
			{
				// ----FAIL----
				Log.Warning("Unknown state.");
				callback.Invoke(false);
			}

			Log.Debug("Watch time: " + watch.Elapsed.TotalMilliseconds);
		}
	}
}
