using UnityEngine;

public class Make_Weapon : CardType
{
    public Make_Weapon(CardData dataFile) : base(dataFile)
    {
    }

    public override void ForPlayer(Player player)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Weapon));
    }
}
