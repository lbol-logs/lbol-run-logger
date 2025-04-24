namespace RunLogger.Utils.RunLogLib.Entities
{
    internal class CardObjWithPrice : CardObj
    {
        internal int? Price { get; set; }
        internal bool? IsDiscounted { get; set; }
    }
}