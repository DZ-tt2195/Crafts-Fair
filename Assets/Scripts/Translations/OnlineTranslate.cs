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
public static string Online_Add_Score (string Player,string Num) => $"Online_Add_Score\tPlayer\t{Player}\tNum\t{Num}";
public static string Online_Lose_Score (string Player,string Num) => $"Online_Lose_Score\tPlayer\t{Player}\tNum\t{Num}";
public static string Online_Add_Token (string Player,string Num,string Token) => $"Online_Add_Token\tPlayer\t{Player}\tNum\t{Num}\tToken\t{Token}";
public static string Online_Remove_Token (string Player,string Num,string Token) => $"Online_Remove_Token\tPlayer\t{Player}\tNum\t{Num}\tToken\t{Token}";
public static string Online_Draw_Placard (string Player,string Card) => $"Online_Draw_Placard\tPlayer\t{Player}\tCard\t{Card}";
public static string Online_Draw_Placard_Others (string Player) => $"Online_Draw_Placard_Others\tPlayer\t{Player}";
public static string Online_Discard_Placard (string Player,string Card) => $"Online_Discard_Placard\tPlayer\t{Player}\tCard\t{Card}";
public static string Online_Make_Submission (string Player,string Num1,string Num2) => $"Online_Make_Submission\tPlayer\t{Player}\tNum1\t{Num1}\tNum2\t{Num2}";
}
public enum OnlinePackage {Online_Player_Playing,Online_Player_Spectating,Online_Player_Reconnected,Online_Player_Disconnected,Online_Player_Quit,Online_Waiting_on_Players,Online_Tie_Game,Online_Player_Resigned,Online_Player_Won,Online_Next_Card,Online_Add_Score,Online_Lose_Score,Online_Add_Token,Online_Remove_Token,Online_Draw_Placard,Online_Draw_Placard_Others,Online_Discard_Placard,Online_Make_Submission}
