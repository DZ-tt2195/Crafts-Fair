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

    protected override void Awake()
    {
        base.Awake();
        this.bottomType = this.GetType();
        inst = this;
        PhotonNetwork.AutomaticallySyncScene = true;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

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

            int forcedPlacards = 4;
            for (int i = 1; i<=forcedPlacards; i++)
            {
                int chosenNumber = PlayerPrefs.GetInt($"Twist {i}");
                if (chosenNumber >= 0 && placardIDs.Remove(chosenNumber))
                    placardIDs.Insert(0, chosenNumber);
            }

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

#region Misc

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

    public void CreateStartingDeck()
    {
        List<int> startingProgress = new();
        List<int> startingIDs = new();
        for (int i = 0; i<4; i++)
        {
            GameObject nextCard = MakeObject(cardPrefab.gameObject);
            PhotonView cardPV = nextCard.GetComponent<PhotonView>();

            startingProgress.Add(cardPV.ViewID);
            startingIDs.Add(0);                    
        }
        for (int i = 0; i<2; i++)
        {
            GameObject nextCard = MakeObject(cardPrefab.gameObject);
            PhotonView cardPV = nextCard.GetComponent<PhotonView>();

            startingProgress.Add(cardPV.ViewID);
            startingIDs.Add(1);                    
        }
        DoFunction(() => CreateCards("Starting", startingProgress.ToArray(), startingIDs.ToArray()));

        List<int> twistDeck = new();
        List<int> twistIDs = new();
        for (int i = 0; i<GameFiles.inst.twistFiles.Count; i++)
        {
            GameObject nextCard = MakeObject(cardPrefab.gameObject);
            PhotonView cardPV = nextCard.GetComponent<PhotonView>();
            twistDeck.Add(cardPV.ViewID);
            twistIDs.Add(i);
        }
        twistIDs = twistIDs.Shuffle();

        int forcedTwists = 4;
        for (int i = 1; i<=forcedTwists; i++)
        {
            int chosenNumber = PlayerPrefs.GetInt($"Twist {i}");
            if (chosenNumber >= 0 && twistIDs.Remove(chosenNumber))
                twistIDs.Insert(0, chosenNumber);
        }

        DoFunction(() => CreateCards("Twist", twistDeck.ToArray(), twistIDs.ToArray()));

        int[] chosenTwists = new int[forcedTwists];
        for (int i = 0; i<forcedTwists; i++)
        {
            startingProgress.Add(twistDeck[i]);
            chosenTwists[i] = twistDeck[i];
        }
        startingProgress = startingProgress.Shuffle();
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
            vertical = true;
        }
        else if (typeToFind.Equals("Starting"))
        {
            toFind = GameFiles.inst.startingFiles;
            vertical = true;
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

    #endregion

}
