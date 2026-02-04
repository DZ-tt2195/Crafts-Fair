using UnityEngine;

public class Make_Tech : CardType
{
    public Make_Tech(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        if (MyExtensions.SumOfArray(player.GetTokenDict()[TokenType.TechIcon]) == 0)
            player.AddLoseToken(4, (1, TokenType.TechIcon), logged);
    }
}
