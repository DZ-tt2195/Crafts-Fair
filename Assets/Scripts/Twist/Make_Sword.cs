using UnityEngine;

public class Make_Sword : CardType
{
    public Make_Sword(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Sword));
    }
}
