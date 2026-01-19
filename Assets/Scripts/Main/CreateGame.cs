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
                MakePlayerAndCards();

                if (PhotonNetwork.CurrentRoom.Players.Count == (int)GetRoomProperty(ConstantStrings.CanPlay))
                    InstantChangeRoomProp(ConstantStrings.JoinAsSpec, true, false);
            }
        }
        else
        {
            PlayerPrefs.DeleteKey(ConstantStrings.LastRoom);
            InstantChangeRoomProp(ConstantStrings.CanPlay, 1);
            MakePlayerAndCards();
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1.5f);
            RefreshUI(true);
        }

        void MakePlayerAndCards()
        {
            int nextPlayerPosition = (int)GetRoomProperty(ConstantStrings.NextPlayerPosition);
            InstantChangeRoomProp(ConstantStrings.NextPlayerPosition, nextPlayerPosition + 1);

            ExitGames.Client.Photon.Hashtable playerProps = new()
            {
                [ConstantStrings.Waiting] = false,
                [ConstantStrings.MyPosition] = nextPlayerPosition,
            };

            List<int> startingPlacardDeck = new();
            List<int> placardID = new();

            for (int i = 0; i < GameFiles.inst.placardFiles.Count; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    GameObject nextCard = MakeObject(cardPrefab.gameObject);
                    PhotonView cardPV = nextCard.GetComponent<PhotonView>();

                    startingPlacardDeck.Add(cardPV.ViewID);
                    placardID.Add(i);
                }
            }
            DoFunction(() => CreatePlacards(startingPlacardDeck.ToArray(), placardID.ToArray()));
            startingPlacardDeck = startingPlacardDeck.Shuffle();
            playerProps.Add(ConstantStrings.MyDeck, startingPlacardDeck.ToArray());
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
            MakeObject(playerPrefab.gameObject);
        }
    }

    [PunRPC]
    void CreatePlacards(int[] arrayOfPVs, int[] cardNames)
    {
        for (int i = 0; i<arrayOfPVs.Length; i++)
        {
            GameObject obj = PhotonView.Find(arrayOfPVs[i]).gameObject;
            obj.GetComponent<Card>().AssignCard(GameFiles.inst.placardFiles[cardNames[i]], 0f);
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
        int numTakeTurn = 5;
        int numGainPlacard = 2;

        for (int i = 0; i<numTakeTurn; i++)
        {
            GameObject nextCard = MakeObject(cardPrefab.gameObject);
            PhotonView cardPV = nextCard.GetComponent<PhotonView>();

            startingProgress.Add(cardPV.ViewID);
            startingIDs.Add(0);                    
        }
        for (int i = 0; i<numGainPlacard; i++)
        {
            GameObject nextCard = MakeObject(cardPrefab.gameObject);
            PhotonView cardPV = nextCard.GetComponent<PhotonView>();

            startingProgress.Add(cardPV.ViewID);
            startingIDs.Add(1);                    
        }
        DoFunction(() => CreateStartings(startingProgress.ToArray(), startingIDs.ToArray()));

        //create twists

        startingProgress = startingProgress.Shuffle();
        InstantChangeRoomProp(ConstantStrings.ProgressDeck, startingProgress.ToArray());        
    }

    [PunRPC]
    void CreateStartings(int[] arrayOfPVs, int[] cardNames)
    {
        for (int i = 0; i<arrayOfPVs.Length; i++)
        {
            GameObject obj = PhotonView.Find(arrayOfPVs[i]).gameObject;
            obj.GetComponent<Card>().AssignCard(GameFiles.inst.startingFiles[cardNames[i]], 0f);
        }
    }

    #endregion

}
