using UnityEngine;

public class Make_Bone : CardType
{
    public Make_Bone(CardData dataFile) : base(dataFile)
    {
    }

    public override void ForPlayer(Player player)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Bone));
    }
}
