using UnityEngine;

public class Museums : CardType
{
    public Museums(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        int[] allHouses = player.GetTokenDict()[TokenType.HouseIcon];
        for (int i = allHouses.Length-1; i >= 0; i--)
        {
            if (allHouses[i] >= 1)
            {
                player.AddLoseToken(1, (i, TokenType.ArtIcon), logged);
                break;
            }
        }
    }
}
