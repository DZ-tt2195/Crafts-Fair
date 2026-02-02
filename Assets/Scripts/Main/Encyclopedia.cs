using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using MyBox;
using Photon.Pun;

public class Encyclopedia : MonoBehaviour
{
    public static Encyclopedia inst;
    [SerializeField] Card buyerPrefab;
    [SerializeField] Card eventPrefab;
    [SerializeField] GridLayoutGroup buyerGrid;
    [SerializeField] GridLayoutGroup eventGrid;
    [SerializeField] RectTransform buyerView;
    [SerializeField] RectTransform eventView;
    [SerializeField] Slider viewSlider;
    List<Card> allBuyers = new();
    List<Card> allEvents = new();

    private void Awake()
    {
        inst = this;
        viewSlider.onValueChanged.AddListener(Change);
        Change(0);

        void Change(float value)
        {
            buyerView.gameObject.SetActive((int)value == 0);
            eventView.gameObject.SetActive((int)value == 1);
        }
    }

    private void Start()
    {
        for (int i = 0; i < GameFiles.inst.buyerFiles.Count; i++)
        {
            GameObject nextCard = Instantiate(buyerPrefab.gameObject);
            Card cardPV = nextCard.GetComponent<Card>();
            cardPV.AssignCard(GameFiles.inst.buyerFiles[i], 1f, true, Vector3.one);
            allBuyers.Add(cardPV);
            cardPV.transform.SetParent(buyerGrid.transform);
        }
        for (int i = 0; i < GameFiles.inst.eventFiles.Count; i++)
        {
            GameObject nextCard = Instantiate(eventPrefab.gameObject);
            Card cardPV = nextCard.GetComponent<Card>();
            cardPV.AssignCard(GameFiles.inst.eventFiles[i], 1f, false, Vector3.one);
            allEvents.Add(cardPV);
            cardPV.transform.SetParent(eventGrid.transform);
        }
    }
}
