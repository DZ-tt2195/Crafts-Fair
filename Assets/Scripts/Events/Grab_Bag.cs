using UnityEngine;

public class Grab_Bag : CardType
{
    public Grab_Bag(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        player.AddLoseToken(1, (1, TokenType.ArtIcon), logged);
        player.AddLoseToken(1, (1, TokenType.HouseIcon), logged);
        player.AddLoseToken(1, (1, TokenType.SwordIcon), logged);
        player.AddLoseToken(1, (1, TokenType.TechIcon), logged);
    }
}
