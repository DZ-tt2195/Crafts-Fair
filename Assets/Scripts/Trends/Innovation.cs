using UnityEngine;

public class Innovation : CardType
{
    public Innovation(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        if (MyExtensions.SumOfArray(player.GetTokenDict()[TokenType.TechIcon]) == 0)
            player.AddLoseToken(4, (1, TokenType.TechIcon), logged);
    }
}
