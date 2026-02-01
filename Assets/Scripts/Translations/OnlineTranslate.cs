public static class OnlineTranslate 
{
public static string Online_Player_Playing (string Player) => $"Online_Player_Playing\tPlayer\t{Player}";
public static string Online_Player_Spectating (string Player) => $"Online_Player_Spectating\tPlayer\t{Player}";
public static string Online_Player_Reconnected (string Player) => $"Online_Player_Reconnected\tPlayer\t{Player}";
public static string Online_Player_Disconnected (string Player) => $"Online_Player_Disconnected\tPlayer\t{Player}";
public static string Online_Player_Quit (string Player) => $"Online_Player_Quit\tPlayer\t{Player}";
public static string Online_Waiting_on_Players (string Num) => $"Online_Waiting_on_Players\tNum\t{Num}";
public static string Online_Next_Turn (string Number) => $"Online_Next_Turn\tNumber\t{Number}";
public static string Online_Chose_Token (string Player,string Token) => $"Online_Chose_Token\tPlayer\t{Player}\tToken\t{Token}";
public static string Online_Add_Score (string Player,string Num) => $"Online_Add_Score\tPlayer\t{Player}\tNum\t{Num}";
public static string Online_Lose_Score (string Player,string Num) => $"Online_Lose_Score\tPlayer\t{Player}\tNum\t{Num}";
public static string Online_Add_Token (string Player,string Amount,string Token,string Level) => $"Online_Add_Token\tPlayer\t{Player}\tAmount\t{Amount}\tToken\t{Token}\tLevel\t{Level}";
public static string Online_Remove_Token (string Player,string Amount,string Token,string Level) => $"Online_Remove_Token\tPlayer\t{Player}\tAmount\t{Amount}\tToken\t{Token}\tLevel\t{Level}";
public static string Online_Upgrade_Token (string Player,string Amount,string Token,string Level1,string Level2) => $"Online_Upgrade_Token\tPlayer\t{Player}\tAmount\t{Amount}\tToken\t{Token}\tLevel1\t{Level1}\tLevel2\t{Level2}";
public static string Online_Downgrade_Token (string Player,string Amount,string Token,string Level1,string Level2) => $"Online_Downgrade_Token\tPlayer\t{Player}\tAmount\t{Amount}\tToken\t{Token}\tLevel1\t{Level1}\tLevel2\t{Level2}";
public static string Online_Draw_Placard (string Player,string Card) => $"Online_Draw_Placard\tPlayer\t{Player}\tCard\t{Card}";
public static string Online_Draw_Placard_Others (string Player) => $"Online_Draw_Placard_Others\tPlayer\t{Player}";
public static string Online_Discard_Placard (string Player,string Card) => $"Online_Discard_Placard\tPlayer\t{Player}\tCard\t{Card}";
public static string Online_Make_Submission (string Player,string TokenNum,string CardNum) => $"Online_Make_Submission\tPlayer\t{Player}\tTokenNum\t{TokenNum}\tCardNum\t{CardNum}";
public static string Online_No_Submission (string Player) => $"Online_No_Submission\tPlayer\t{Player}";
public static string Online_Events_To_Resolve (string Num) => $"Online_Events_To_Resolve\tNum\t{Num}";
public static string Online_Resolve_Card (string Player,string Card) => $"Online_Resolve_Card\tPlayer\t{Player}\tCard\t{Card}";
public static string Online_Tie_Game () => $"Online_Tie_Game";
public static string Online_Player_Resigned (string Player) => $"Online_Player_Resigned\tPlayer\t{Player}";
public static string Online_Player_Won (string Player) => $"Online_Player_Won\tPlayer\t{Player}";
}
