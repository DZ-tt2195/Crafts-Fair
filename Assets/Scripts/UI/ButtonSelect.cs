using UnityEngine;
using MyBox;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSelect : MonoBehaviour
{
    public Button button { get; private set; }
    [SerializeField] Image border;

    private void Awake()
    {
        button = GetComponent<Button>();
        SetBorder(false);
    }
    public void SetBorder(bool border) => SetBorder(border, Color.white);
    public void SetBorder(bool border, Color color)
    {
        this.border.gameObject.SetActive(border);
        this.border.color = color;
    }

    private void FixedUpdate()
    {
        try { this.border.SetAlpha(CreateGame.inst.opacity); } catch { }
    }
}
