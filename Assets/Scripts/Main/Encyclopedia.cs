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
    [SerializeField] ListUI customerList;
    [SerializeField] ListUI twistList;
    [SerializeField] Slider viewSlider;
    List<Card> allCustomers = new();
    List<Card> allTwists = new();
    [Foldout("Texts", true)]
    [SerializeField] TMP_Text customer;
    [SerializeField] TMP_Text twist;
    [SerializeField] TMP_Text close;

    private void Awake()
    {
        inst = this;
        viewSlider.onValueChanged.AddListener(Change);
        Change(0);

        void Change(float value)
        {
            customerList.mainThing.gameObject.SetActive((int)value == 0);
            twistList.mainThing.gameObject.SetActive((int)value == 1);
        }
    }
    private void Start()
    {
        customer.text = AutoTranslate.Customer();
        twist.text = AutoTranslate.Twist();
        close.text = AutoTranslate.Close();

        for (int i = 0; i < GameFiles.inst.customerFiles.Count; i++)
        {
            Card nextCard = Instantiate(customerList.prefab).GetComponent<Card>();
            nextCard.AssignCard(GameFiles.inst.customerFiles[i], 1f, true, Vector3.one);
            allCustomers.Add(nextCard);
            nextCard.transform.SetParent(customerList.storePrefabs.transform);
        }
        for (int i = 0; i < GameFiles.inst.twistFiles.Count; i++)
        {
            Card nextCard = Instantiate(twistList.prefab).GetComponent<Card>();
            nextCard.AssignCard(GameFiles.inst.twistFiles[i], 1f, false, Vector3.one);
            allTwists.Add(nextCard);
            nextCard.transform.SetParent(twistList.storePrefabs.transform);
        }
    }
}
