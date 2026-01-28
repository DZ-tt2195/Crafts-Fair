using UnityEngine;

public class Make_Sword : CardType
{
    public Make_Sword(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.AddRemoveToken(4, (1, TokenType.SwordIcon), logged);
    }
}
