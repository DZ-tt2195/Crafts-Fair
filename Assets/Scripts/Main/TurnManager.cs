using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using TMPro;
using System.Linq;

public class TurnManager : PhotonCompatible
{

#region Setup

    public static TurnManager inst;
    Dictionary<Player, ExitGames.Client.Photon.Hashtable> playerPropertyToChange;
    ExitGames.Client.Photon.Hashtable masterPropertyToChange;
    Dictionary<string, Turn> storedTurns = new();
    [SerializeField] Transform endScreen;
    [SerializeField] TMP_Text summaryText;

    protected override void Awake()
    {
        base.Awake();
        inst = this;
        this.bottomType = this.GetType();
        endScreen.gameObject.SetActive(false);
        playerPropertyToChange = new();
        masterPropertyToChange = new();
    }

    #endregion

#region Turns
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        bool HasPropertyAndValue(ExitGames.Client.Photon.Hashtable changedProps, string propertyName, object expected)
        {
            return (changedProps.ContainsKey(propertyName.ToString()) && changedProps[propertyName.ToString()].Equals(expected));
        }

        if (HasPropertyAndValue(changedProps, ConstantStrings.Waiting, true))
        {
            (List<Photon.Realtime.Player> players, List<Photon.Realtime.Player> spectators) = GetPlayers(false);
            
            int WaitingOnPlayers()
            {
                int playersWaiting = (int)GetRoomProperty(ConstantStrings.CanPlay);
                List<Photon.Realtime.Player> isWaiting = new();
                isWaiting.AddRange(spectators);

                foreach (Photon.Realtime.Player player in players)
                {
                    if ((bool)GetPlayerProperty(player, ConstantStrings.Waiting))
                    {
                        isWaiting.Add(player);
                        playersWaiting--;
                    }
                }

                UpdateWaitingText(isWaiting, playersWaiting);
                return playersWaiting;
            }

            if (PhotonNetwork.IsMasterClient && WaitingOnPlayers() == 0 && !(bool)GetRoomProperty(ConstantStrings.GameOver))
            {
                foreach (Photon.Realtime.Player nextPlayer in players)
                    DoFunction(() => SharePropertyChanges(), nextPlayer);
                storedTurns[GetCurrentPhase()].MasterEnd();

                UpdateWaitingText(spectators, players.Count);
                Invoke(nameof(NextPhase), 0.5f);
            }
        }
    }

    void UpdateWaitingText(List<Photon.Realtime.Player> toSend, int playersWaiting)
    {
        foreach (Photon.Realtime.Player player in toSend)
            MakeDecision.inst.DoFunction(() => MakeDecision.inst.PackagedInstructions(OnlineTranslate.Online_Waiting_on_Players(playersWaiting.ToString())), player);
    }

    void NextPhase()
    {
        (Player, int) highestScore = (null, 0);
        foreach (Player player in CreateGame.inst.listOfPlayers)
        {
            int health = player.GetScore();
            if (health > highestScore.Item2)
                highestScore = (player, health);
            else if (health == highestScore.Item2)
                highestScore = (null, health);
        }

        if (highestScore.Item2 >= 20 && highestScore.Item1 != null)
        {
            TextForEnding(OnlineTranslate.Online_Player_Won(highestScore.Item1.name), -1);
            InstantChangeRoomProp(ConstantStrings.CurrentPhase, nameof(Ending));
        }
        else
        {
            string currentPhase = (string)GetRoomProperty(ConstantStrings.CurrentPhase);
            string nextPhase = (string)GetRoomProperty(ConstantStrings.NextPhase);

            List<Card> deck = GetCardList(ConstantStrings.ProgressDeck);
            List<Card> discard = GetCardList(ConstantStrings.ProgressDiscard);

            if (currentPhase.Equals(nameof(ResolveCard)))
            {
                Card top = deck[0];
                deck.RemoveAt(0);
                discard.Add(top);
            }
            if (nextPhase.Equals(nameof(ResolveCard)) && deck.Count == 0)
            {
                InstantChangeRoomProp(ConstantStrings.Shuffle, GetInt(ConstantStrings.Shuffle)+1);
                Log.inst.MasterText(true, AutoTranslate.Blank());
                Log.inst.MasterText(true, OnlineTranslate.Online_Shuffle_Deck());
                deck = discard.Shuffle();
                discard.Clear();
            }
            InstantChangeRoomProp(ConstantStrings.ProgressDeck, ConvertCardList(deck));
            InstantChangeRoomProp(ConstantStrings.ProgressDiscard, ConvertCardList(discard));

            InstantChangeRoomProp(ConstantStrings.NextPhase, nameof(ResolveCard));
            InstantChangeRoomProp(ConstantStrings.CurrentPhase, nextPhase);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(ConstantStrings.CurrentPhase))
        {
            Debug.Log($"switched to {GetCurrentPhase()}");
            if (PhotonNetwork.IsMasterClient)
                storedTurns[GetCurrentPhase()].MasterStart();

            CreateGame.inst.RefreshUI(true);
            foreach (Player player in CreateGame.inst.listOfPlayers)
            {
                if (player.photonView.AmOwner)
                    player.StartTurn();
            }
        }
    }

    #endregion

#region Property Helpers

    object FindThisProperty(string property, Player player)
    {
        if (player != null && !playerPropertyToChange.ContainsKey(player))
            playerPropertyToChange.Add(player, new());

        if (masterPropertyToChange.ContainsKey(property))
            return masterPropertyToChange[property];
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(property))
            return GetRoomProperty(property);

        if (playerPropertyToChange[player].ContainsKey(property))
            return playerPropertyToChange[player][property];
        else
            return GetPlayerProperty(player.photonView.Owner, property);
    }
    public int GetInt(string property, Player player) => (int)FindThisProperty(property, player);
    public int GetInt(string property) => (int)FindThisProperty(property, null);
    public int[] GetIntArray(string property, Player player) => (int[])FindThisProperty(property, player);
    public int[] GetIntArray(string property) => (int[])FindThisProperty(property, null);
    public List<string> GetStringList(string property, Player player)
    {
        string[] stringArray = (string[])FindThisProperty(property, player);
        return stringArray.ToList();
    }
    public List<Card> GetCardList(string property, Player player) => ConvertIntArray((int[])FindThisProperty(property, player));
    public List<Card> GetCardList(string property) => ConvertIntArray((int[])FindThisProperty(property, null));
    List<Card> ConvertIntArray(int[] arrayOfPVs)
    {
        if (arrayOfPVs == null)
            return new();

        List<Card> listOfCards = new();
        foreach (int nextPV in arrayOfPVs)
            listOfCards.Add(PhotonView.Find(nextPV).GetComponent<Card>());
        return listOfCards;
    }
    public int[] ConvertCardList(List<Card> listOfCards)
    {
        int[] arrayOfCards = new int[listOfCards.Count];
        for (int i = 0; i < arrayOfCards.Length; i++)
            arrayOfCards[i] = listOfCards[i].photonView.ViewID;
        return arrayOfCards;
    }

    #endregion

#region Change Properties
    string GetCurrentPhase()
    {
        try
        {
            string toReturn = (string)GetRoomProperty(ConstantStrings.CurrentPhase);
            if (!storedTurns.ContainsKey(toReturn))
                storedTurns.Add(toReturn, (Turn)Activator.CreateInstance(Type.GetType(toReturn)));
            return toReturn;
        }
        catch
        {
            return nameof(WaitForJoiners);
        }
    }
    public (string, Action) GetTurnAction(Player player)
    {
        string currentPhase = GetCurrentPhase();
        return (currentPhase, () => storedTurns[currentPhase].ForPlayer(player));
    }
    public Card TopCard()
    {
        List<Card> deck = GetCardList(ConstantStrings.ProgressDeck); 
        return deck[0];       
    }
    public void WillChangePlayerProperty(Player player, string playerProperty, object changeInto)
    {
        if (!playerPropertyToChange.ContainsKey(player))
            playerPropertyToChange.Add(player, new());

        if (playerPropertyToChange[player].ContainsKey(playerProperty.ToString()))
            playerPropertyToChange[player][playerProperty.ToString()] = changeInto;
        else
            playerPropertyToChange[player].Add(playerProperty.ToString(), changeInto);
    }

    public void WillChangeMasterProperty(string masterProperty, object changeInto)
    {
        if (masterPropertyToChange.ContainsKey(masterProperty))
            masterPropertyToChange[masterProperty] = changeInto;
        else
            masterPropertyToChange.Add(masterProperty, changeInto);
    }

    [PunRPC]
    void SharePropertyChanges()
    {
        Log.inst.ShareTexts();
        int currentPosition = (int)GetPlayerProperty(PhotonNetwork.LocalPlayer, ConstantStrings.MyPosition);

        foreach (var KVP in playerPropertyToChange)
        {
            KVP.Key.photonView.Owner.SetCustomProperties(KVP.Value);
            KVP.Value.Clear();
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(masterPropertyToChange);
        masterPropertyToChange.Clear();
    }

    #endregion

#region Ending

    public void TextForEnding(string packagedText, int resignPosition)
    {
        Log.inst.MasterText(true, packagedText);
        InstantChangeRoomProp(ConstantStrings.GameOver, true);
        DoFunction(() => ShowEnding(resignPosition), RpcTarget.All);
    }

    [PunRPC]
    void ShowEnding(int resignPosition)
    {
        endScreen.gameObject.SetActive(true);
        string text = "";

        foreach (Player player in CreateGame.inst.listOfPlayers)
        {
            text += $"{player.name}";
            if (player.myPosition == resignPosition)
                text += AutoTranslate.Resigned();
            text += "\n";

/*
            List<string> cardsPlayed = GetStringList(ConstantStrings.AllCardsPlayed, player);
            for (int i = 0; i<cardsPlayed.Count; i++)
            {
                string[] splitUp = cardsPlayed[i].Split('-');

                text += Translator.inst.Packaging("Played_Card_Info", "", splitUp[0], splitUp[1]);
                text += ",";
            }
            text += "\n\n";
            */
        }
        summaryText.text = KeywordTooltip.instance.EditText(text);
    }

#endregion

}
