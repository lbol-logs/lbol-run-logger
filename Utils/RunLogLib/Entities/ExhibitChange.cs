namespace RunLogger.Utils.RunLogLib.Entities
{
    public class ExhibitChange
    {
        public string Id { get; internal set; }
        public int? Counter { get; internal set; }
        public string Type { get; internal set; }
        public int Station { get; internal set; }
    }
}