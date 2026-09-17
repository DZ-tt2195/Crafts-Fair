using UnityEngine;

public class Produce : CardType
{
    public Produce(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        if (WithLevel(player.GetTokenDict(), FindNumber.Minimum, 6, 1))
            player.CreateLoseToken(4, (1, TokenType.ToolIcon), logged);
    }
}
