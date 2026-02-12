using UnityEngine;

public class Tipping : CardType
{
    public Tipping(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        int getPlacards = TurnManager.inst.GetInt(ConstantStrings.BuyersSold, player);
        player.CoinRPC(Mathf.FloorToInt(getPlacards/2f), logged);
    }
}
