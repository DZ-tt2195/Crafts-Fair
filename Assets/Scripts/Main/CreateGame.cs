using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using Photon.Realtime;
using System.Collections;
using MyBox;
using UnityEngine.UI;
using TMPro;
using System.Linq;
[Serializable]
public class EventVisual
{
    public Card card;
    public TMP_Text countText;
    public TokenType token;
}
public class CreateGame : PhotonCompatible
{

#region Setup

    public static CreateGame inst;
    [Foldout("Players", true)]
    List<Player> listOfPlayers = new();
    [ReadOnly] public Player mainPlayer;
    [SerializeField] Player playerPrefab;
    [SerializeField] Card cardPrefab;
    [SerializeField] TMP_Dropdown playerDropdown;

    [Foldout("UI and Animation", true)]
    public Camera mainCamera;
    public float opacity { get; private set; }
    bool decrease = true;
    public Canvas canvas { get; private set; }
    [SerializeField] List<EventVisual> eventInfo = new();

    protected override void Awake()
    {
        base.Awake();
        this.bottomType = this.GetType();
        inst = this;
        PhotonNetwork.AutomaticallySyncScene = true;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        VisualCards((int[])GetRoomProperty(ConstantStrings.EventList));
        playerDropdown.onValueChanged.AddListener(SwitchToPlayer);

        if (!PhotonNetwork.OfflineMode)
        {
            string playerName = PlayerPrefs.GetString(ConstantStrings.MyUserName);

            if (PlayerPrefs.GetString(ConstantStrings.LastRoom).Equals(PhotonNetwork.CurrentRoom.Name))
            {
                CommHub.inst.ShareMessageRPC(OnlineTranslate.Online_Player_Reconnected(playerName), true);
            }
            else if ((bool)GetRoomProperty(ConstantStrings.JoinAsSpec))
            {
                CommHub.inst.ShareMessageRPC(OnlineTranslate.Online_Player_Spectating(playerName), true);
                ExitGames.Client.Photon.Hashtable playerProps = new()
                {
                    [ConstantStrings.Waiting] = true,
                    [ConstantStrings.Playing] = false,
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
                StartCoroutine(Wait());
            }
            else
            {
                CommHub.inst.ShareMessageRPC(OnlineTranslate.Online_Player_Playing(playerName), true);
                PlayerPrefs.SetString(ConstantStrings.LastRoom, PhotonNetwork.CurrentRoom.Name);
                StartCoroutine(MakePlayerAndCards());
                
                if (GetPlayers(false).Item1.Count == (int)GetRoomProperty(ConstantStrings.CanPlay))
                    InstantChangeRoomProp(ConstantStrings.JoinAsSpec, true, false);
            }
        }
        else
        {
            PlayerPrefs.DeleteKey(ConstantStrings.LastRoom);
            InstantChangeRoomProp(ConstantStrings.CanPlay, 1);
            StartCoroutine(MakePlayerAndCards());
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1.5f);
            RefreshUI(true);
        }

        IEnumerator MakePlayerAndCards()
        {
            yield return new WaitForSeconds(1f);
            while (CardMenu.instance.gameObject.activeSelf)
            {
                yield return null;
            }

            List<int> startingPlacardDeck = new();
            List<int> placardIDs = new();
            for (int i = 0; i<GameFiles.inst.buyerFiles.Count; i++)
            {
                GameObject nextCard = MakeObject(cardPrefab.gameObject);
                PhotonView cardPV = nextCard.GetComponent<PhotonView>();
                startingPlacardDeck.Add(cardPV.ViewID);
                placardIDs.Add(i);
            }
            placardIDs = placardIDs.Shuffle();

            DoFunction(() => CreateCards("Placard", startingPlacardDeck.ToArray(), placardIDs.ToArray()));
            InstantChangePlayerProp(PhotonNetwork.LocalPlayer, ConstantStrings.MyDeck, startingPlacardDeck.ToArray());
            MakeObject(playerPrefab.gameObject);
        }
    }

    #endregion

#region Online

    public void Leave()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.LeaveRoom(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        OnLeftRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("0. Loading");
    }

    #endregion

#region UI

    private void FixedUpdate()
    {
        if (decrease)
            opacity -= 0.05f;
        else
            opacity += 0.05f;
        if (opacity < 0 || opacity > 1)
            decrease = !decrease;
    }
    public void RefreshUI(bool forced)
    {
        Log.inst.ChangeScrolling();
        foreach (Player player in listOfPlayers)
            player.UpdateUI(forced);
    }
    public void SwitchToPlayer(Player player) => playerDropdown.value = listOfPlayers.IndexOf(player);
    public void SwitchToPlayer(int value)
    {
        Debug.Log($"switching to player {value}");
        foreach (Player next in listOfPlayers)
            next.transform.localPosition = new Vector3(-10000, -10000);
        listOfPlayers[value].transform.localPosition = Vector3.zero;
    }
    public List<Player> GetPlayers() => listOfPlayers;
    public void AddPlayerRPC(Player player)
    {
        DoFunction(() => AddPlayer(player.photonView.ViewID), RpcTarget.AllBuffered);
    }
    [PunRPC]
    void AddPlayer(int playerID)
    {
        Player player = PhotonView.Find(playerID).GetComponent<Player>();
        listOfPlayers.Add(player);
        player.transform.SetParent(canvas.transform);
        player.transform.SetAsFirstSibling();

        playerDropdown.AddOptions(new List<TMP_Dropdown.OptionData>() { new(player.name) });
        if (listOfPlayers.Count == (int)GetRoomProperty(ConstantStrings.CanPlay))
        {
            playerDropdown.gameObject.SetActive(playerDropdown.options.Count >= 2);
            int myPosition = GetThisPlayerPosition(PhotonNetwork.LocalPlayer);
            int index = listOfPlayers.IndexOf(mainPlayer);

            if (myPosition == -1 || index == 0)
                SwitchToPlayer(0);
            else
                playerDropdown.value = index;
        }
    }

#endregion 

#region  Events
    public void CreateEvents()
    {
        List<int> EventIDs = new();
        for (int i = 0; i<GameFiles.inst.trendFiles.Count; i++)
            EventIDs.Add(i);
        EventIDs = EventIDs.Shuffle();

        int forcedEvents = 4;
        for (int i = 1; i<=forcedEvents; i++)
        {
            int chosenNumber = PlayerPrefs.GetInt($"Event {i}");
            if (chosenNumber >= 0 && EventIDs.Remove(chosenNumber))
                EventIDs.Insert(0, chosenNumber);
        }

        int[] chosenEvents = new int[forcedEvents];
        for (int i = 0; i<forcedEvents; i++)
            chosenEvents[i] = EventIDs[i];
        InstantChangeRoomProp(ConstantStrings.EventList, chosenEvents.ToArray());
    }

    [PunRPC]
    void CreateCards(string typeToFind, int[] arrayOfPVs, int[] cardNames)
    {
        List<CardData> toFind = new();
        bool vertical = false;
        if (typeToFind.Equals("Event"))
        {
            toFind = GameFiles.inst.trendFiles;
            vertical = false;
        }
        else if (typeToFind.Equals("Placard"))
        {
            toFind = GameFiles.inst.buyerFiles;
            vertical = true;
        }

        for (int i = 0; i<arrayOfPVs.Length; i++)
        {
            GameObject obj = PhotonView.Find(arrayOfPVs[i]).gameObject;
            obj.GetComponent<Card>().AssignCard(toFind[cardNames[i]], 0f, vertical, Vector3.one);
        }
    }
    void VisualCards(int[] cardIDs)
    {
        for (int i = 0; i<cardIDs.Length; i++)
        {
            eventInfo[i].card.gameObject.SetActive(true);
            CardData data = GameFiles.inst.trendFiles[cardIDs[i]];
            eventInfo[i].card.AssignCard(data, 1, false, new(0.5f, 0.5f, 0.5f));
        }
        for (int i = cardIDs.Length; i<eventInfo.Count; i++)
        {
            eventInfo[i].card.gameObject.SetActive(false);
        }
        UpdateTexts();
    }

    public EventVisual GetEvent(TokenType type)
    {
        foreach (EventVisual tv in eventInfo)
        {
            if (tv.token == type)
                return tv;
        }
        return null;
    }
    void UpdateTexts()
    {
        foreach (EventVisual visual in eventInfo)
        {
            string tokenText = ConstantStrings.TokenCounter(visual.token);
            visual.countText.text = KeywordTooltip.instance.EditText($"{TurnManager.inst.GetInt(tokenText)}{Translator.inst.Translate(visual.token.ToString())}");
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(ConstantStrings.EventList))
        {    
            VisualCards((int[])propertiesThatChanged[ConstantStrings.EventList]);
        }
        else
        {
            foreach (TokenType token in Enum.GetValues(typeof(TokenType)))
            {
                string changedTokenText = ConstantStrings.TokenCounter(token);
                if (propertiesThatChanged.ContainsKey(changedTokenText))
                {
                    UpdateTexts();
                    return;
                }
            }   
        }
    }
    #endregion

}
