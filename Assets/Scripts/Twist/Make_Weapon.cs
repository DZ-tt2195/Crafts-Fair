using UnityEngine;

public class Make_Weapon : CardType
{
    public Make_Weapon(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Weapon));
    }
}
