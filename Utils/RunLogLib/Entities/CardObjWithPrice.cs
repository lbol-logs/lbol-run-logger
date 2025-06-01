namespace RunLogger.Utils.RunLogLib.Entities
{
    public class CardObjWithPrice : CardObj
    {
        public int? Price { get; set; }
        public bool? IsDiscounted { get; set; }
    }
}