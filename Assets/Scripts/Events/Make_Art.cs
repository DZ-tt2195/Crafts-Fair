using UnityEngine;

public class Make_Art : CardType
{
    public Make_Art(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        if (player.GetCoins() >= 5)
            player.AddLoseToken(4, (1, TokenType.ArtIcon), logged);
    }
}
