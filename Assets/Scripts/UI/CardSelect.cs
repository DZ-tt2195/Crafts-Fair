using UnityEngine;
using UnityEngine.UI;
using MyBox;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class CardSelect : MonoBehaviour
{
    [SerializeField]CardLayout layout;
    [SerializeField]Button randomButton;
    [SerializeField]Button chooseButton;
    [SerializeField] TMP_Text randomText;
    [SerializeField] TMP_Text chooseText;
    List<CardData> allData;
    bool vertical;

    private void Awake()
    {
        vertical = false;
        allData = GameFiles.inst.trendFiles;
        randomText.text = AutoTranslate.Random();
        chooseText.text = AutoTranslate.Choose();
        randomButton.onClick.AddListener(() => SetCardImage(-1));
        chooseButton.onClick.AddListener(() => CardMenu.instance.ChooseFromList(this, allData, vertical));
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(this.name) && PlayerPrefs.GetInt(this.name) >= 0)
            SetCardImage(PlayerPrefs.GetInt(this.name));
        else
            SetCardImage(-1);
    }

    public void SetCardImage(int number)
    {
        if (number < 0)
        {
            PlayerPrefs.SetInt(this.name, -1);
            layout.FillInCards(null, 0, vertical);
        }
        else
        {
            PlayerPrefs.SetInt(this.name, number);
            layout.FillInCards(allData[number], 1, vertical);
        }
        PlayerPrefs.Save();
    }
}
