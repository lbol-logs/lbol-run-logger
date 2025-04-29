using LBoL.Core.Cards;
using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils
{
    internal static class EntitiesManager
    {
        internal static void AddCardChange(IEnumerable<Card> cards, ChangeType changeType)
        {
            foreach (Card card in cards)
            {
                CardChange cardChange = new CardChange
                {
                    Id = card.Id,
                    IsUpgraded = card.IsUpgraded,
                    UpgradeCounter = card.UpgradeCounter,
                    Type = changeType.ToString(),
                    Station = Controller.CurrentStationIndex
                };
                Controller.Instance.RunLog.Cards.Add(cardChange);
            }
        }
    }
}
