using UnityEngine;

public class Make_Art : CardType
{
    public Make_Art(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Art));
    }
}
