using UnityEngine;

public class Make_Sword : CardType
{
    public Make_Sword(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        if (WithLevel(player.GetTokenDict(), FindNumber.Minimum, 6, 1))
            player.AddLoseToken(4, (1, TokenType.SwordIcon), logged);
    }
}
