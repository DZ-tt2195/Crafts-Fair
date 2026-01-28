using UnityEngine;

public class Make_Tech : CardType
{
    public Make_Tech(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Tech));
    }
}
