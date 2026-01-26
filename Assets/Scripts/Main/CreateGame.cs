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
public class TwistVisual
{
    public Card card;
    public TMP_Text countText;
    public TokenType type;
}
public class CreateGame : PhotonCompatible
{

#region Setup

    public static CreateGame inst;
    [Foldout("Players", true)]
    [ReadOnly] public List<Player> listOfPlayers = new();
    [SerializeField] Player playerPrefab;
    [SerializeField] Card cardPrefab;

    [Foldout("UI and Animation", true)]
    public Camera mainCamera;
    public float opacity { get; private set; }
    bool decrease = true;
    public Canvas canvas { get; private set; }
    [SerializeField] List<TwistVisual> twistInfo = new();
    protected override void Awake()
    {
        base.Awake();
        this.bottomType = this.GetType();
        inst = this;
        PhotonNetwork.AutomaticallySyncScene = true;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        VisualCards((int[])GetRoomProperty(ConstantStrings.TwistList));

        if (!PhotonNetwork.OfflineMode)
        {
            string playerName = PlayerPrefs.GetString(ConstantStrings.MyUserName);

            if (PlayerPrefs.GetString(ConstantStrings.LastRoom).Equals(PhotonNetwork.CurrentRoom.Name))
            {
                CommHub.inst.ShareMessageRPC(OnlineTranslate.Online_Player_Disconnected(playerName), true);
            }
            else if ((bool)GetRoomProperty(ConstantStrings.JoinAsSpec))
            {
                CommHub.inst.ShareMessageRPC(OnlineTranslate.Online_Player_Spectating(playerName), true);
                ExitGames.Client.Photon.Hashtable playerProps = new()
                {
                    [ConstantStrings.Waiting] = true,
                    [ConstantStrings.MyPosition] = -1,
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
                StartCoroutine(Wait());
            }
            else
            {
                CommHub.inst.ShareMessageRPC(OnlineTranslate.Online_Player_Playing(playerName), true);
                PlayerPrefs.SetString(ConstantStrings.LastRoom, PhotonNetwork.CurrentRoom.Name);
                StartCoroutine(MakePlayerAndCards());
                
                if (PhotonNetwork.CurrentRoom.Players.Count == (int)GetRoomProperty(ConstantStrings.CanPlay))
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
            int nextPlayerPosition = (int)GetRoomProperty(ConstantStrings.NextPlayerPosition);
            InstantChangeRoomProp(ConstantStrings.NextPlayerPosition, nextPlayerPosition + 1);

            yield return new WaitForSeconds(1f);
            while (CardMenu.instance.gameObject.activeSelf)
            {
                yield return null;
            }

            ExitGames.Client.Photon.Hashtable playerProps = new()
            {
                [ConstantStrings.Waiting] = false,
                [ConstantStrings.MyPosition] = nextPlayerPosition,
            };

            List<int> startingPlacardDeck = new();
            List<int> placardIDs = new();
            for (int i = 0; i<GameFiles.inst.placardFiles.Count; i++)
            {
                GameObject nextCard = MakeObject(cardPrefab.gameObject);
                PhotonView cardPV = nextCard.GetComponent<PhotonView>();
                startingPlacardDeck.Add(cardPV.ViewID);
                placardIDs.Add(i);
            }
            placardIDs = placardIDs.Shuffle();

            DoFunction(() => CreateCards("Placard", startingPlacardDeck.ToArray(), placardIDs.ToArray()));
            playerProps.Add(ConstantStrings.MyDeck, startingPlacardDeck.ToArray());
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
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
#endregion 

#region  Twists
    public void CreateTwists()
    {
        List<int> twistIDs = new();
        for (int i = 0; i<GameFiles.inst.twistFiles.Count; i++)
            twistIDs.Add(i);
        twistIDs = twistIDs.Shuffle();

        int forcedTwists = 4;
        for (int i = 1; i<=forcedTwists; i++)
        {
            int chosenNumber = PlayerPrefs.GetInt($"Twist {i}");
            if (chosenNumber >= 0 && twistIDs.Remove(chosenNumber))
                twistIDs.Insert(0, chosenNumber);
        }

        int[] chosenTwists = new int[forcedTwists];
        for (int i = 0; i<forcedTwists; i++)
            chosenTwists[i] = twistIDs[i];
        InstantChangeRoomProp(ConstantStrings.TwistList, chosenTwists.ToArray());
    }

    [PunRPC]
    void CreateCards(string typeToFind, int[] arrayOfPVs, int[] cardNames)
    {
        List<CardData> toFind = new();
        bool vertical = false;
        if (typeToFind.Equals("Twist"))
        {
            toFind = GameFiles.inst.twistFiles;
            vertical = false;
        }
        else if (typeToFind.Equals("Placard"))
        {
            toFind = GameFiles.inst.placardFiles;
            vertical = true;
        }

        for (int i = 0; i<arrayOfPVs.Length; i++)
        {
            GameObject obj = PhotonView.Find(arrayOfPVs[i]).gameObject;
            obj.GetComponent<Card>().AssignCard(toFind[cardNames[i]], 0f, vertical);
        }
    }
    void VisualCards(int[] cardIDs)
    {
        for (int i = 0; i<cardIDs.Length; i++)
        {
            twistInfo[i].card.gameObject.SetActive(true);
            CardData data = GameFiles.inst.twistFiles[cardIDs[i]];
            twistInfo[i].card.AssignCard(data, 1, false);
            twistInfo[i].countText.text = $"{TurnManager.inst.GetInt(ConstantStrings.TokenCounter(twistInfo[i].type))}";
        }
        for (int i = cardIDs.Length; i<twistInfo.Count; i++)
        {
            twistInfo[i].card.gameObject.SetActive(false);
        }
    }

    public TwistVisual GetTwist(TokenType type)
    {
        foreach (TwistVisual tv in twistInfo)
        {
            if (tv.type == type)
                return tv;
        }
        return null;
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(ConstantStrings.TwistList))
        {    
            VisualCards((int[])propertiesThatChanged[ConstantStrings.TwistList]);
        }
        else
        {
            foreach (TokenType type in Enum.GetValues(typeof(TokenType)))
            {
                string changedTokenText = ConstantStrings.TokenCounter(type);
                if (propertiesThatChanged.ContainsKey(changedTokenText))
                    GetTwist(type).countText.text = $"{(int)propertiesThatChanged[changedTokenText]}";
            }        
        }
    }
    #endregion

}
