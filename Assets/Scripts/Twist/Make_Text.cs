using UnityEngine;

public class Make_Text : CardType
{
    public Make_Text(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Text));
    }
}
