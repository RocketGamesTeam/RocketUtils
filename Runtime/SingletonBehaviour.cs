using UnityEngine;

namespace RocketUtils
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected bool DrawDebugLabel;
        protected bool DrawTimeLabel;
        protected bool DrawFPSLabel;
        
        private static T _instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)FindObjectOfType(typeof(T));
                if (Instance == null)
                {
                    Debug.LogWarning("An instance of " + typeof(T) +
                                   " is needed in the scene, but there is none.");
                }
                else
                {
                    OnAwake();
                }
            }
            else if (Instance != this)
            {
                Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
            }
        }        

        protected abstract void OnAwake();

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (!_instance)
                    {
                        Debug.LogWarning("An instance of " + typeof(T) +
                                       " is needed in the scene, but there is none.");
                    }
                    else
                    {
                        SingletonBehaviour<T> instance = _instance as SingletonBehaviour<T>;
                        instance.OnAwake();
                    }
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

#if ROC_DEBUG_MODE || ROC_LOG_MODE
        private GUIStyle _timeGUIStyle;
        private GUIStyle _fpsGUIStyle;
        private float _deltaTime;
        
        private string labelString = "Rocket Debug";

        public void ModifyDebugLabel(string textToAppend)
        {
#if ROC_DEBUG_MODE
            labelString += textToAppend;
#endif // ROC_DEBUG_MODE
        }
        
        void OnGUI()
        {
            if (!DrawDebugLabel) return;

// #if ROC_DEBUG_MODE
            // labelString = "Rocket Debug";
// #endif

            DrawOutline(new Rect(35, Screen.height - 130, 300, 80), labelString, 2, new GUIStyle
            {
                fontSize = 24
            });
            GUI.Label(new Rect(35, Screen.height - 130, 300, 80), labelString, new GUIStyle
            {
                normal = { textColor = Color.white },
                fontSize = 24
            });

            if (DrawFPSLabel)
            {
                if (_fpsGUIStyle == null)
                {
                    _fpsGUIStyle = new GUIStyle
                    {
                        fontSize = 24,
                        active = { textColor = Color.white },
                        normal = { textColor = Color.white },
                        focused = { textColor = Color.white },
                        hover = { textColor = Color.white },
                    };
                }
                _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
                
                float fps = 1.0f / _deltaTime;
        
                GUILayout.Space(50);
                
                GUILayout.BeginHorizontal();
                GUILayout.Space(100);
                GUILayout.BeginVertical("box");
                
                GUILayout.Label($"{fps:00}" + " FPS", _fpsGUIStyle);
                
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            
            if (DrawTimeLabel)
            {
                if (_timeGUIStyle == null)
                {
                    _timeGUIStyle = new GUIStyle
                    {
                        fontSize = 28,
                        active = { textColor = Color.white },
                        normal = { textColor = Color.white },
                        focused = { textColor = Color.white },
                        hover = { textColor = Color.white },

                    };
                }
        
                GUI.contentColor = Color.white;
        
                if (!DrawFPSLabel)
                    GUILayout.Space(50);
                if (DrawFPSLabel)
                    GUILayout.Space(5);
                GUILayout.BeginHorizontal("box");
        
                int seconds = (int)Time.time;
                int minute = (seconds / 60);
                int hour = (minute / 60);
        
                GUILayout.Label($"Time : {hour:00}:{(minute) % 60:00}:{seconds % 60:00}", _timeGUIStyle);

                GUILayout.EndHorizontal();
            }
        }

        void DrawOutline(Rect r, string t, int strength, GUIStyle style)
        {
            GUI.color = new Color(0, 0, 0, 1);
            int i;
            for (i = -strength; i <= strength; i++)
            {
                GUI.Label(new Rect(r.x - strength, r.y + i, r.width, r.height), t, style);
                GUI.Label(new Rect(r.x + strength, r.y + i, r.width, r.height), t, style);
            }
            for (i = -strength + 1; i <= strength - 1; i++)
            {
                GUI.Label(new Rect(r.x + i, r.y - strength, r.width, r.height), t, style);
                GUI.Label(new Rect(r.x + i, r.y + strength, r.width, r.height), t, style);
            }
            GUI.color = new Color(1, 1, 1, 1);
        }
#endif
    }
}