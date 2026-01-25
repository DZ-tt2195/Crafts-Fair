using UnityEngine;

public class Make_Bone : CardType
{
    public Make_Bone(CardData dataFile) : base(dataFile)
    {
    }

    public override bool WillTrigger(Player player, TwistTrigger trigger)
    {
        return trigger == TwistTrigger.StartTurn && (TurnManager.inst.GetInt(ConstantStrings.TurnNumber) % 2 == 0);
    }

    public override void TwistEffect(Player player, int logged)
    {
        player.ChangeTokenRPC(2, (1, TokenType.Bone));
    }
}
