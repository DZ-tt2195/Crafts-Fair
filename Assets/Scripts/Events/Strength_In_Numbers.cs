using UnityEngine;

public class Strength_In_Numbers : CardType
{
    public Strength_In_Numbers(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        int getPlacards = TurnManager.inst.GetInt(ConstantStrings.BuyersSold, player);
        player.CoinRPC(Mathf.FloorToInt(getPlacards/2f), logged);
    }
}
