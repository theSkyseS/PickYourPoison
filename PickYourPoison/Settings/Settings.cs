namespace PickYourPoison.Settings
{
    public enum Mode
    {
        Vanilla,
        CACO,
        AA
    }
    public class Settings
    {
        public Mode RunMode { get; set; } = Mode.Vanilla;
        public bool LongDescriptions { get; set; } = true;
    }
}