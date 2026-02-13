using UnityEngine;
using UnityEngine.UI;
using MyBox;
using System.Linq;
using System.Collections.Generic;
using Photon.Pun;
using System.Text.RegularExpressions;
using TMPro;

public class CardMenu : PhotonCompatible
{
    public static CardMenu instance;
    [Foldout("UI", true)]
    int step = 0;
    [SerializeField] Button confirmButton;
    [SerializeField] GridLayoutGroup storeButtons;
    CardSelect mostRecentClick;
    List<(CardLayout, Button)> blankButtons = new();
    [SerializeField] List<CardSelect> eventSelect = new();
    [Foldout("Text", true)]
    [SerializeField] TMP_Text chooseTrends;
    [SerializeField] TMP_Text trendArt;
    [SerializeField] TMP_Text trendHouse;
    [SerializeField] TMP_Text trendSword;
    [SerializeField] TMP_Text trendTech;
    [SerializeField] TMP_Text confirm;

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
            foreach (CardSelect select in eventSelect)
                select.SetCardImage(-1);
            this.gameObject.SetActive(false);
        }
        else
        {
            Advance();
            confirmButton.onClick.AddListener(Advance);
        }
    }
    public void ChooseFromList(CardSelect clicked, List<CardData> allData, bool vertical)
    {
        mostRecentClick = clicked;
        for (int i = 0; i < blankButtons.Count; i++)
        {
            (CardLayout layout, Button button) = blankButtons[i];
            try
            {
                layout.FillInCards(allData[i], 1, vertical);
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
            Translations();
        }
        else
        {
            PlayerPrefs.Save();
            this.gameObject.SetActive(false);
        }
        step++;
    }
    void Translations()
    {
        chooseTrends.text = AutoTranslate.Choose_Twists();
        trendArt.text = KeywordTooltip.instance.EditText(AutoTranslate.Custom_Art_Twist());
        trendHouse.text = KeywordTooltip.instance.EditText(AutoTranslate.Custom_House_Twist());
        trendSword.text = KeywordTooltip.instance.EditText(AutoTranslate.Custom_Tool_Twist());
        trendTech.text = KeywordTooltip.instance.EditText(AutoTranslate.Custom_Book_Twist());
        confirm.text = AutoTranslate.Confirm();
    }
}
