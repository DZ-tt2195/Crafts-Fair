public static class AutoTranslate 
{ 

public static string Player_Count (string Current,string Max) => Translator.inst.Translate("Player_Count", new() {("Current", Current),("Max", Max)});

public static string Attempt_to_reconnect (string Room) => Translator.inst.Translate("Attempt_to_reconnect", new() {("Room", Room)});

public static string Failed_to_reconnect (string Room) => Translator.inst.Translate("Failed_to_reconnect", new() {("Room", Room)});

public static string Player_Playing (string Player) => Translator.inst.Translate("Player_Playing", new() {("Player", Player)});

public static string Player_Spectating (string Player) => Translator.inst.Translate("Player_Spectating", new() {("Player", Player)});

public static string Player_Reconnected (string Player) => Translator.inst.Translate("Player_Reconnected", new() {("Player", Player)});

public static string Player_Disconnected (string Player) => Translator.inst.Translate("Player_Disconnected", new() {("Player", Player)});

public static string Player_Quit (string Player) => Translator.inst.Translate("Player_Quit", new() {("Player", Player)});

public static string Waiting_on_Players (string Num) => Translator.inst.Translate("Waiting_on_Players", new() {("Num", Num)});

public static string Choose_One_Instruction (string Card) => Translator.inst.Translate("Choose_One_Instruction", new() {("Card", Card)});

public static string Discard_Instruction (string Card) => Translator.inst.Translate("Discard_Instruction", new() {("Card", Card)});

public static string Target_Instruction (string Player,string Card) => Translator.inst.Translate("Target_Instruction", new() {("Player", Player),("Card", Card)});

public static string Pick_Player (string Player) => Translator.inst.Translate("Pick_Player", new() {("Player", Player)});

public static string Player_Resigned (string Player) => Translator.inst.Translate("Player_Resigned", new() {("Player", Player)});

public static string Player_Lost (string Player) => Translator.inst.Translate("Player_Lost", new() {("Player", Player)});

public static string Next_Card (string Card) => Translator.inst.Translate("Next_Card", new() {("Card", Card)});

public static string Game_Designer() => Translator.inst.Translate("Game_Designer");
public static string Last_Update() => Translator.inst.Translate("Last_Update");
public static string Translator_Credit() => Translator.inst.Translate("Translator_Credit");
public static string Language() => Translator.inst.Translate("Language");
public static string Loading() => Translator.inst.Translate("Loading");
public static string Update_History() => Translator.inst.Translate("Update_History");
public static string Upload_Translation() => Translator.inst.Translate("Upload_Translation");
public static string Download_English() => Translator.inst.Translate("Download_English");
public static string Select_Region() => Translator.inst.Translate("Select_Region");
public static string US_West_Coast() => Translator.inst.Translate("US_West_Coast");
public static string US_East_Coast() => Translator.inst.Translate("US_East_Coast");
public static string Europe() => Translator.inst.Translate("Europe");
public static string Asia() => Translator.inst.Translate("Asia");
public static string Single_Player() => Translator.inst.Translate("Single_Player");
public static string Connect() => Translator.inst.Translate("Connect");
public static string Enter_username() => Translator.inst.Translate("Enter_username");
public static string Disconnect() => Translator.inst.Translate("Disconnect");
public static string Disconnected_from_server() => Translator.inst.Translate("Disconnected_from_server");
public static string Failed_to_connect_to_server() => Translator.inst.Translate("Failed_to_connect_to_server");
public static string Reconnect() => Translator.inst.Translate("Reconnect");
public static string Online_Tutorial_1() => Translator.inst.Translate("Online_Tutorial_1");
public static string Online_Tutorial_2() => Translator.inst.Translate("Online_Tutorial_2");
public static string Create() => Translator.inst.Translate("Create");
public static string Create_Room_with_players() => Translator.inst.Translate("Create_Room_with_players");
public static string Enter_hostname() => Translator.inst.Translate("Enter_hostname");
public static string Join() => Translator.inst.Translate("Join");
public static string Type_in_username() => Translator.inst.Translate("Type_in_username");
public static string Encyclopedia() => Translator.inst.Translate("Encyclopedia");
public static string Close() => Translator.inst.Translate("Close");
public static string Type_into_chat() => Translator.inst.Translate("Type_into_chat");
public static string Undo() => Translator.inst.Translate("Undo");
public static string Short() => Translator.inst.Translate("Short");
public static string Long() => Translator.inst.Translate("Long");
public static string Confirm() => Translator.inst.Translate("Confirm");
public static string Decline() => Translator.inst.Translate("Decline");
public static string Blank() => Translator.inst.Translate("Blank");
public static string Game_Over() => Translator.inst.Translate("Game_Over");
public static string Leave() => Translator.inst.Translate("Leave");
public static string Tie_Game() => Translator.inst.Translate("Tie_Game");
public static string Resigned() => Translator.inst.Translate("Resigned");
}
public enum ToTranslate {
Game_Designer,Last_Update,Translator_Credit,Language,Loading,Update_History,Upload_Translation,Download_English,Select_Region,US_West_Coast,US_East_Coast,Europe,Asia,Single_Player,Connect,Enter_username,Disconnect,Disconnected_from_server,Failed_to_connect_to_server,Reconnect,Online_Tutorial_1,Online_Tutorial_2,Create,Create_Room_with_players,Enter_hostname,Join,Type_in_username,Encyclopedia,Close,Type_into_chat,Undo,Short,Long,Confirm,Decline,Blank,Game_Over,Leave,Tie_Game,Resigned
}
