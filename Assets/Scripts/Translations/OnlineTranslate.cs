public static class OnlineTranslate 
{
public static string Online_Player_Playing (string Player) => $"Online_Player_Playing\tPlayer\t{Player}";
public static string Online_Player_Spectating (string Player) => $"Online_Player_Spectating\tPlayer\t{Player}";
public static string Online_Player_Reconnected (string Player) => $"Online_Player_Reconnected\tPlayer\t{Player}";
public static string Online_Player_Disconnected (string Player) => $"Online_Player_Disconnected\tPlayer\t{Player}";
public static string Online_Player_Quit (string Player) => $"Online_Player_Quit\tPlayer\t{Player}";
public static string Online_Waiting_on_Players (string Num) => $"Online_Waiting_on_Players\tNum\t{Num}";
public static string Online_Tie_Game () => $"Online_Tie_Game";
public static string Online_Player_Resigned (string Player) => $"Online_Player_Resigned\tPlayer\t{Player}";
public static string Online_Player_Won (string Player) => $"Online_Player_Won\tPlayer\t{Player}";
public static string Online_Next_Card (string Card) => $"Online_Next_Card\tCard\t{Card}";
}
public enum OnlinePackage {Online_Player_Playing,Online_Player_Spectating,Online_Player_Reconnected,Online_Player_Disconnected,Online_Player_Quit,Online_Waiting_on_Players,Online_Tie_Game,Online_Player_Resigned,Online_Player_Won,Online_Next_Card}
