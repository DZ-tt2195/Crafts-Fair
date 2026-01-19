using UnityEngine;
using UnityEngine.UI;
using MyBox;
using System.Linq;
using System.Collections.Generic;
using Photon.Pun;
using System.Text.RegularExpressions;

public class CardMenu : PhotonCompatible
{
    public static CardMenu instance;
    int step = 0;
    [SerializeField] Button confirmButton;
    [SerializeField] GridLayoutGroup storeButtons;
    CardSelect mostRecentClick;
    List<(CardLayout, Button)> blankButtons = new();
    [SerializeField] List<CardSelect> placardSelect = new();
    [SerializeField] List<CardSelect> twistSelect = new();

    protected override void Awake()
    {
        base.Awake();
        this.bottomType = this.GetType();
        instance = this;
    }

    private void Start()
    {
        string currentPhase = (string)GetRoomProperty(ConstantStrings.CurrentPhase);
        if (!(AmMaster() && currentPhase.Equals(nameof(WaitForJoiners))))
        {
            foreach (CardSelect select in placardSelect)
                select.SetCardImage(-1);
            foreach (CardSelect select in twistSelect)
                select.SetCardImage(-1);
            this.gameObject.SetActive(false);
        }
        else
        {
            if ((int)GetRoomProperty(ConstantStrings.CanPlay) >= 2)
            {
                foreach (CardSelect select in placardSelect)
                    select.SetCardImage(-1);
            }
            Advance();
            confirmButton.onClick.AddListener(Advance);
        }
    }

    public void ChooseFromList(CardSelect clicked, List<CardData> allData, bool vertical)
    {
        mostRecentClick = clicked;
        if (vertical)
        {
            storeButtons.constraintCount = 4;
            storeButtons.spacing = new(30, 30);
        }
        else
        {
            storeButtons.constraintCount = 2;
            storeButtons.spacing = new(200, -80);
        }

        for (int i = 0; i < blankButtons.Count; i++)
        {
            (CardLayout layout, Button button) = blankButtons[i];
            try
            {
                layout.FillInCards(allData[i], 1, vertical ? 0 : -90);
                button.gameObject.SetActive(true);
            }
            catch
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    void SendName(int number)
    {
        mostRecentClick.SetCardImage(number);
        mostRecentClick = null;
        foreach (var thing in blankButtons)
            thing.Item1.gameObject.SetActive(false);
    }

    void Advance()
    {
        if (step == 0)
        {
            for (int i = 0; i < storeButtons.transform.childCount; i++)
            {
                Button nextButton = storeButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
                blankButtons.Add((nextButton.GetComponent<CardLayout>(), nextButton));
                nextButton.interactable = true;
                nextButton.onClick.RemoveAllListeners();
                int number = i;
                nextButton.onClick.AddListener(() => SendName(number));
                nextButton.gameObject.SetActive(false);
            }
        }
        else
        {
            PlayerPrefs.Save();
            this.gameObject.SetActive(false);
        }
        step++;
    }
}
