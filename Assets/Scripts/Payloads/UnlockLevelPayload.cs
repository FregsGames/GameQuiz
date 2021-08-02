
namespace Assets.Scripts.Payloads
{
    public class UnlockLevelPayload
    {
        public string LevelCompletedID { get; set; }

        public UnlockLevelPayload(string levelCompletedID)
        {
            LevelCompletedID = levelCompletedID;
        }
    }
}
