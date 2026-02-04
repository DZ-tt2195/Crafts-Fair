using UnityEngine;

public class High_Art : CardType
{
    public High_Art(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        int[] allSwords = player.GetTokenDict()[TokenType.SwordIcon];
        for (int i = allSwords.Length-1; i >= 0; i--)
        {
            if (allSwords[i] >= 1)
            {
                player.AddLoseToken(1, (i, TokenType.ArtIcon), logged);
                break;
            }
        }
    }
}
