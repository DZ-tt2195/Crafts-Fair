using UnityEngine;

public class Make_Art : CardType
{
    public Make_Art(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.AddRemoveToken(4, (1, TokenType.ArtIcon), logged);
    }
}
