using UnityEngine;

public class Hustle : CardType
{
    public Hustle(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        int getPlacards = TurnManager.inst.GetInt(ConstantStrings.BuyersSold, player);
        player.CoinRPC(Mathf.FloorToInt(getPlacards/2f), logged);
    }
}
