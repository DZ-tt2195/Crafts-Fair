using UnityEngine;

public class Museums : CardType
{
    public Museums(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        int highestHouse = 0;
        int[] allHouses = player.GetTokenDict()[TokenType.HouseIcon];
        for (int i = allHouses.Length-1; i >= 0; i--)
        {
            if (allHouses[i] >= 1)
            {
                highestHouse = i-1;
                break;
            }
        }
        player.AddLoseToken(1, (1, TokenType.ArtIcon), logged);
        player.UpDowngradeToken(1, (1, TokenType.ArtIcon), highestHouse, logged);
    }
}
