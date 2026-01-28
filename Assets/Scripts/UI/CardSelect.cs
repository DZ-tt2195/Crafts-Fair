using UnityEngine;
using UnityEngine.UI;
using MyBox;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CardSelect : MonoBehaviour
{
    [SerializeField] RectTransform rectTrans;
    [SerializeField]CardLayout layout;
    [SerializeField]Button randomButton;
    [SerializeField]Button chooseButton;
    List<CardData> allData;
    bool vertical;

    private void Awake()
    {
        vertical = false;
        allData = GameFiles.inst.twistFiles;
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
