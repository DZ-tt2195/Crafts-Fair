using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using MyBox;

public enum TokenType { ArtIcon, HouseIcon, SwordIcon, TechIcon}
public class TokenDisplay : MonoBehaviour
{
    public ButtonSelect selectMe { get; private set; }
    [SerializeField] TMP_Text description;
    public (int value, TokenType type) info {get; private set;}

    private void Awake()
    {
        selectMe = GetComponent<ButtonSelect>();
    }

    public void ChangeInfo(int value, TokenType token, string text)
    {
        description.text = text;
        info = (value, token);
    }
}
