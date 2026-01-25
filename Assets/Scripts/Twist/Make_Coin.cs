using UnityEngine;

public class Make_Coin : CardType
{
    public Make_Coin(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Coin));
    }
}
