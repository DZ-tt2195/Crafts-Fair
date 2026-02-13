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
    [SerializeField] Card customerPrefab;
    [SerializeField] Card strategyPrefab;
    [SerializeField] GridLayoutGroup customerGrid;
    [SerializeField] GridLayoutGroup strategyGrid;
    [SerializeField] RectTransform customerView;
    [SerializeField] RectTransform strategyView;
    [SerializeField] Slider viewSlider;
    List<Card> allcustomers = new();
    List<Card> allStrategies = new();
    [Foldout("Texts", true)]
    [SerializeField] TMP_Text customer;
    [SerializeField] TMP_Text strategy;
    [SerializeField] TMP_Text close;

    private void Awake()
    {
        inst = this;
        viewSlider.onValueChanged.AddListener(Change);
        Change(0);

        void Change(float value)
        {
            customerView.gameObject.SetActive((int)value == 0);
            strategyView.gameObject.SetActive((int)value == 1);
        }
    }
    private void Start()
    {
        Translations();
        for (int i = 0; i < GameFiles.inst.customerFiles.Count; i++)
        {
            GameObject nextCard = Instantiate(customerPrefab.gameObject);
            Card cardPV = nextCard.GetComponent<Card>();
            cardPV.AssignCard(GameFiles.inst.customerFiles[i], 1f, true, Vector3.one);
            allcustomers.Add(cardPV);
            cardPV.transform.SetParent(customerGrid.transform);
        }
        for (int i = 0; i < GameFiles.inst.strategyFiles.Count; i++)
        {
            GameObject nextCard = Instantiate(strategyPrefab.gameObject);
            Card cardPV = nextCard.GetComponent<Card>();
            cardPV.AssignCard(GameFiles.inst.strategyFiles[i], 1f, false, Vector3.one);
            allStrategies.Add(cardPV);
            cardPV.transform.SetParent(strategyGrid.transform);
        }
    }
    void Translations()
    {
        customer.text = AutoTranslate.Customer();
        strategy.text = AutoTranslate.Strategy();
        close.text = AutoTranslate.Close();
    }
}
