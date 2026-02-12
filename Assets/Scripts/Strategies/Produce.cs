using UnityEngine;

public class Produce : CardType
{
    public Produce(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        if (WithLevel(player.GetTokenDict(), FindNumber.Minimum, 6, 1))
            player.AddLoseToken(4, (1, TokenType.ToolIcon), logged);
    }
}
