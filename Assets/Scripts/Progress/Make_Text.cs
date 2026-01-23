using UnityEngine;

public class Make_Text : CardType
{
    public Make_Text(CardData dataFile) : base(dataFile)
    {
    }

    public override void ForPlayer(Player player)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Text));
    }
}
