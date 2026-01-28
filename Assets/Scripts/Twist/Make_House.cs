using UnityEngine;

public class Make_House : CardType
{
    public Make_House(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.AddRemoveToken(4, (1, TokenType.HouseIcon), logged);
    }
}
