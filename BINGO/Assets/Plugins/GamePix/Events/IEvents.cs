namespace GamePix.Events
{
    public interface IEvents
    {
        /// <summary>
        /// Call this function every time there's a score update inside the Game.
        /// </summary>
        /// <param name="score">Score</param>
        void UpdateScore(int score); 
    }
}