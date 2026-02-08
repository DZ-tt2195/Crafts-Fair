using UnityEngine;

public class Security : CardType
{
    public Security(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        if (WithLevel(player.GetTokenDict(), FindNumber.Minimum, 6, 1))
            player.AddLoseToken(4, (1, TokenType.SwordIcon), logged);
    }
}
