using UnityEngine;

public class Reprint : CardType
{
    public Reprint(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        if (MyExtensions.SumOfArray(player.GetTokenDict()[TokenType.BookIcon]) == 0)
            player.AddLoseToken(4, (1, TokenType.BookIcon), logged);
    }
}
