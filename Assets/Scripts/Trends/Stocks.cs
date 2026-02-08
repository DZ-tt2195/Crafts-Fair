using UnityEngine;

public class Stocks : CardType
{
    public Stocks(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        int getPlacards = TurnManager.inst.GetInt(ConstantStrings.BuyersSold, player);
        player.CoinRPC(Mathf.FloorToInt(getPlacards/2f), logged);
    }
}
