using UnityEngine;

public class Make_Coin : CardType
{
    public Make_Coin(CardData dataFile) : base(dataFile)
    {
    }

    public override void ForPlayer(Player player)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Coin));
    }
}
