using UnityEngine;

public class Make_House : CardType
{
    public Make_House(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.ChangeTokenRPC(2, (1, TokenType.House));
    }
}
