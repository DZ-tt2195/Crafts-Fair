using UnityEngine;

public class Grab_Bag : CardType
{
    public Grab_Bag(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.AddRemoveToken(1, (1, TokenType.ArtIcon), logged);
        player.AddRemoveToken(1, (1, TokenType.HouseIcon), logged);
        player.AddRemoveToken(1, (1, TokenType.SwordIcon), logged);
        player.AddRemoveToken(1, (1, TokenType.TechIcon), logged);
    }
}
