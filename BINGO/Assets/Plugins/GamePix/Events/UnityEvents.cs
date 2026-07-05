#if UNITY_EDITOR || !UNITY_WEBGL
namespace GamePix.Events
{
    public class UnityEvents: IEvents
    {
        public void UpdateScore(int score)
        {
           Gpx.Log(string.Format("[Gpx] Update score: {0}", score));
        }
    }
}
#endif