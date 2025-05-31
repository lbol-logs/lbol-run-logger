namespace RunLogger.Utils.RunLogLib.Entities
{
    public class CardObjWithPrice : CardObj
    {
        public int? Price { get; internal set; }
        public bool? IsDiscounted { get; internal set; }
    }
}