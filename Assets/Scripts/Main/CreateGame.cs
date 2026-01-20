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
        DoFunction(() => CreateStartings(startingProgress.ToArray(), startingIDs.ToArray()));

        HashSet<int> forcedTwists = new();
        List<int> twistIDs = new();
        int numTwists = 4;
        for (int i = 1; i<=numTwists; i++)
        {
            GameObject nextCard = MakeObject(cardPrefab.gameObject);
            PhotonView cardPV = nextCard.GetComponent<PhotonView>();
            startingProgress.Add(cardPV.ViewID);
            twistIDs.Add(cardPV.ViewID);

            int chosenNumber = PlayerPrefs.GetInt($"Twist {i}");
            if (chosenNumber >= 0)
                forcedTwists.Add(chosenNumber);
        }
        while (forcedTwists.Count < numTwists)
        {
            int randomNum = UnityEngine.Random.Range(0, GameFiles.inst.twistFiles.Count);
            if (!forcedTwists.Contains(randomNum))
                forcedTwists.Add(randomNum);
        }
        DoFunction(() => CreateTwists(twistIDs.ToArray(), forcedTwists.ToArray()));
        MakeDecision.inst.ChangeDisplayedCards(twistIDs.ToArray());

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
    [PunRPC]
    void CreateTwists(int[] arrayOfPVs, int[] cardNames)
    {
        for (int i = 0; i<arrayOfPVs.Length; i++)
        {
            GameObject obj = PhotonView.Find(arrayOfPVs[i]).gameObject;
            obj.GetComponent<Card>().AssignCard(GameFiles.inst.twistFiles[cardNames[i]], 0f);
        }
    }

    #endregion

}
