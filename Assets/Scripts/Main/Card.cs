using UnityEngine;
using MyBox;
using System.Collections;
using System.Linq;
using System;
using Photon.Pun;
using System.Collections.Generic;

public class Card : PhotonCompatible
{

#region Setup

    public CardLayout layout { get; private set; }
    bool flipping;
    bool vertical;
    public CardType thisCard { get; private set; }
    public ButtonSelect selectMe { get; private set; }
    public CardData dataFile {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        this.bottomType = this.GetType();

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        this.transform.localScale = Vector3.Lerp(Vector3.one, canvas.transform.localScale, 0.5f);
        selectMe = GetComponent<ButtonSelect>();
        layout = GetComponent<CardLayout>();

        /*
        if (PhotonNetwork.IsConnected && !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(HealthString()))
        {
            ExitGames.Client.Photon.Hashtable initialProps = new()
            {
                [HealthString()] = 0,
                [StunString()] = new int[0],
                [ProtectString()] = new int[0],
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(initialProps);
        }
        */
    }

    /*public string HealthString() => $"{this.photonView.ViewID}_Health";

    public string StunString() => $"{this.photonView.ViewID}_Stun";

    public string ProtectString() => $"{this.photonView.ViewID}_Protect";
    */
    public void AssignCard(CardData dataFile, float startingAlpha, bool vertical)
    {
        this.dataFile = dataFile;
        this.vertical = vertical;
        thisCard = (CardType)Activator.CreateInstance(Type.GetType(dataFile.cardName), dataFile);
        this.layout.FillInCards(dataFile, startingAlpha, vertical);
        this.name = dataFile.cardName;
        KeywordTooltip.instance.NewCardRC(Translator.inst.Translate(dataFile.cardName), this.layout);
    }

    #endregion

#region Animations

    public void MoveCardRPC(Vector3 newPos, float waitTime, Vector3 newScale)
    {
        StartCoroutine(MoveCard(newPos, waitTime, newScale));
    }

    IEnumerator MoveCard(Vector3 newPos, float waitTime, Vector3 newScale)
    {
        float elapsedTime = 0;
        Vector2 originalPos = this.transform.localPosition;
        Vector2 originalScale = this.transform.localScale;

        while (elapsedTime < waitTime)
        {
            this.transform.localPosition = Vector3.Lerp(originalPos, newPos, elapsedTime / waitTime);
            this.transform.localScale = Vector3.Lerp(originalScale, newScale, elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.transform.localPosition = newPos;
    }

    public void FlipCardRPC(float newAlpha, float totalTime)
    {
        if (!flipping && this.layout.GetAlpha() != newAlpha)
            StartCoroutine(FlipCard(newAlpha, totalTime));
    }

    IEnumerator FlipCard(float newAlpha, float totalTime)
    {
        flipping = true;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        float elapsedTime = 0f;

        Vector3 originalRot = this.transform.localEulerAngles;
        Vector3 newRot = new(0, 90, 0);

        while (elapsedTime < totalTime)
        {
            this.transform.localEulerAngles = Vector3.Lerp(originalRot, newRot, elapsedTime / totalTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.layout.FillInCards(thisCard.dataFile, newAlpha, vertical);
        elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            this.transform.localEulerAngles = Vector3.Lerp(newRot, originalRot, elapsedTime / totalTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.transform.localEulerAngles = originalRot;
        flipping = false;
    }

    #endregion

}