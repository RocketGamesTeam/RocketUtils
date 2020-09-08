using UnityEngine;

namespace RocketUtils
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected bool DrawDebugLabel;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)FindObjectOfType(typeof(T));
                if (Instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) +
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

        public static T Instance { get; private set; }

#if ROC_DEBUG_MODE
        void OnGUI()
        {
            if(!DrawDebugLabel) return;

            DrawTextWithOutline(new Rect(15, Screen.height - 35, 300, 80), "Rocket Debug", new GUIStyle(), Color.white, Color.black, 1);
        }

        void DrawTextWithOutline(Rect centerRect, string text, GUIStyle style, Color borderColor, Color innerColor, int borderWidth)
        {
            // assign the border color
            style.normal.textColor = borderColor;
            style.fontSize = 24;

            // draw an outline color copy to the left and up from original
            Rect modRect = centerRect;
            modRect.x -= borderWidth;
            modRect.y -= borderWidth;
            GUI.Label(modRect, text, style);


            // stamp copies from the top left corner to the top right corner
            while (modRect.x <= centerRect.x + borderWidth)
            {
                modRect.x++;
                GUI.Label(modRect, text, style);
            }

            // stamp copies from the top right corner to the bottom right corner
            while (modRect.y <= centerRect.y + borderWidth)
            {
                modRect.y++;
                GUI.Label(modRect, text, style);
            }

            // stamp copies from the bottom right corner to the bottom left corner
            while (modRect.x >= centerRect.x - borderWidth)
            {
                modRect.x--;
                GUI.Label(modRect, text, style);
            }

            // stamp copies from the bottom left corner to the top left corner
            while (modRect.y >= centerRect.y - borderWidth)
            {
                modRect.y--;
                GUI.Label(modRect, text, style);
            }

            // draw the inner color version in the center
            style.normal.textColor = innerColor;
            GUI.Label(centerRect, text, style);
        }
#endif
    }
}