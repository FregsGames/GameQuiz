namespace Assets.Scripts.Payloads
{
    public class CupSelectedPayload
    {
        public CupScriptable Cup { get; set; }
        public CupDropdown CupDropdown { get; set; }
        public bool endless { get; set; } = false;
    }
}
