using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using MyBox;
using Photon.Pun;

public class Encyclopedia : MonoBehaviour
{
    public static Encyclopedia inst;
    [Foldout("UI", true)]
    [SerializeField] Card buyerPrefab;
    [SerializeField] Card trendPrefab;
    [SerializeField] GridLayoutGroup buyerGrid;
    [SerializeField] GridLayoutGroup trendGrid;
    [SerializeField] RectTransform buyerView;
    [SerializeField] RectTransform trendView;
    [SerializeField] Slider viewSlider;
    List<Card> allBuyers = new();
    List<Card> allTrends = new();
    [Foldout("Texts", true)]
    [SerializeField] TMP_Text buyer;
    [SerializeField] TMP_Text trend;
    [SerializeField] TMP_Text close;

    private void Awake()
    {
        inst = this;
        viewSlider.onValueChanged.AddListener(Change);
        Change(0);

        void Change(float value)
        {
            buyerView.gameObject.SetActive((int)value == 0);
            trendView.gameObject.SetActive((int)value == 1);
        }
    }
    private void Start()
    {
        Translations();
        for (int i = 0; i < GameFiles.inst.buyerFiles.Count; i++)
        {
            GameObject nextCard = Instantiate(buyerPrefab.gameObject);
            Card cardPV = nextCard.GetComponent<Card>();
            cardPV.AssignCard(GameFiles.inst.buyerFiles[i], 1f, true, Vector3.one);
            allBuyers.Add(cardPV);
            cardPV.transform.SetParent(buyerGrid.transform);
        }
        for (int i = 0; i < GameFiles.inst.trendFiles.Count; i++)
        {
            GameObject nextCard = Instantiate(trendPrefab.gameObject);
            Card cardPV = nextCard.GetComponent<Card>();
            cardPV.AssignCard(GameFiles.inst.trendFiles[i], 1f, false, Vector3.one);
            allTrends.Add(cardPV);
            cardPV.transform.SetParent(trendGrid.transform);
        }
    }
    void Translations()
    {
        buyer.text = AutoTranslate.Buyer();
        trend.text = AutoTranslate.Trend();
        close.text = AutoTranslate.Close();
    }
}
