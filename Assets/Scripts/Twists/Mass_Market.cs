using UnityEngine;

public class Mass_Market : CardType
{
    public Mass_Market(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        int getPlacards = TurnManager.inst.GetInt(ConstantStrings.BuyersSold, player);
        player.CoinRPC(Mathf.FloorToInt(getPlacards/2f), logged);
    }
}
