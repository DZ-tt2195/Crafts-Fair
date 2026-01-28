using UnityEngine;

public class Make_Tech : CardType
{
    public Make_Tech(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.AddRemoveToken(4, (1, TokenType.TechIcon), logged);
    }
}
