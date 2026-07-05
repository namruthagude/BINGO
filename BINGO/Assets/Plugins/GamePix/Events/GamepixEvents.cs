#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;

namespace GamePix.Events
{
    public class GamepixEvents : IEvents
    {
        [DllImport("__Internal")]
        private static extern void gpxUpdateScore(int score); 

        public void UpdateScore(int score)
        {
           gpxUpdateScore(score);
        }
    }
}
#endif