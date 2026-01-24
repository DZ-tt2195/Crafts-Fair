using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardLayout : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] CanvasGroup cg;
    [SerializeField] Image cardArt;
    [SerializeField] Image background;
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text textBox;
    CardData storedData;
    bool vertical;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            RightClickedMe(cg.alpha);
    }

    public void RightClickedMe(float alpha)
    {
        PermaUI.inst.RightClickDisplay(storedData, alpha == 1f, vertical);
    }

    public float GetAlpha()
    {
        return cg.alpha;
    }

    public void FillInCards(CardData dataFile, float alpha, bool vertical)
    {
        bool newCard = storedData != dataFile;
        storedData = dataFile;
        cg.alpha = alpha;
        this.vertical = vertical;

        if (dataFile != null && newCard)
        {
            if (dataFile.crownAmount >= 1)
                cardName.text = KeywordTooltip.instance.EditText($"{Translator.inst.Translate(dataFile.cardName)}: {dataFile.crownAmount} {AutoTranslate.CrownIcon()}");
            else
                cardName.text = KeywordTooltip.instance.EditText($"{Translator.inst.Translate(dataFile.cardName)}");
            
            cardArt.sprite = dataFile.sprite;
            textBox.text = KeywordTooltip.instance.EditText(Translator.inst.Translate($"{dataFile.cardName}_Text"));
        }
    }
}
