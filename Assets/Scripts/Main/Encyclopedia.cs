using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using MyBox;
using Photon.Pun;

public class Encyclopedia : MonoBehaviour
{
    public static Encyclopedia inst;
    [SerializeField] Card placardPrefab;
    [SerializeField] Card twistPrefab;
    [SerializeField] GridLayoutGroup placardGrid;
    [SerializeField] GridLayoutGroup twistGrid;
    [SerializeField] RectTransform placardView;
    [SerializeField] RectTransform twistView;
    [SerializeField] Slider viewSlider;
    List<Card> allPlacards = new();
    List<Card> allTwists = new();

    private void Awake()
    {
        inst = this;
        viewSlider.onValueChanged.AddListener(Change);
        Change(0);

        void Change(float value)
        {
            placardView.gameObject.SetActive((int)value == 0);
            twistView.gameObject.SetActive((int)value == 1);
        }
    }

    private void Start()
    {
        for (int i = 0; i < GameFiles.inst.placardFiles.Count; i++)
        {
            GameObject nextCard = Instantiate(placardPrefab.gameObject);
            Card cardPV = nextCard.GetComponent<Card>();
            cardPV.AssignCard(GameFiles.inst.placardFiles[i], 1f, true, Vector3.one);
            allPlacards.Add(cardPV);
            cardPV.transform.SetParent(placardGrid.transform);
        }
        for (int i = 0; i < GameFiles.inst.twistFiles.Count; i++)
        {
            GameObject nextCard = Instantiate(twistPrefab.gameObject);
            Card cardPV = nextCard.GetComponent<Card>();
            cardPV.AssignCard(GameFiles.inst.twistFiles[i], 1f, false, Vector3.one);
            allTwists.Add(cardPV);
            cardPV.transform.SetParent(twistGrid.transform);
        }
    }
}
