using UnityEngine;

public static class ConstantStrings
{
    //player prefs
    public const string MyUserName = nameof(MyUserName);
    public const string LastRoom = nameof(LastRoom);

    //player properties
    public const string Playing = nameof(Playing);
    public const string Waiting = nameof(Waiting);
    public const string MyCoins = nameof(MyCoins);
    public const string MyHand = nameof(MyHand);
    public const string MyDeck = nameof(MyDeck);
    public const string MyDiscard = nameof(MyDiscard);
    public const string ChosenToken = nameof(ChosenToken);
    public const string BuyersSold = nameof(BuyersSold);

    //room properties
    public const string GameName = nameof(GameName);
    public const string CanPlay = nameof(CanPlay);
    public const string GameOver = nameof(GameOver);
    public const string JoinAsSpec = nameof(JoinAsSpec);
    public const string CurrentPhase = nameof(CurrentPhase);
    public const string NextPhase = nameof(NextPhase);
    public const string EventList = nameof(EventList);
    public const string TurnNumber = nameof(TurnNumber);
    public static string TokenCounter(TokenType type) => TokenCounter(type.ToString());
    public static string TokenCounter(string type) => $"TokenCounter{type}";
    public static string MaxLevel => nameof(MaxLevel);
}
