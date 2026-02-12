using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;
using System;
using System.Reflection;
using Photon.Pun;

[Serializable]
public class CardData
{
    public string cardName;
    public int coinAmount = 0;
    public string artCredit;
    public Sprite sprite;
}

public class GameFiles : MonoBehaviour
{
    public static GameFiles inst;
    public List<CardData> customerFiles { get; private set; }
    public List<CardData> strategyFiles { get; private set; }

    void Awake()
    {
        inst = this;
        customerFiles = ReadTSVFile<CardData>(Resources.Load<TextAsset>("Card Info/Customers").text);
        strategyFiles = ReadTSVFile<CardData>(Resources.Load<TextAsset>("Card Info/Strategies").text);
    }

    List<T> ReadTSVFile<T>(string textToConvert) where T : new()
    {
        string[] splitUp = textToConvert.Split('\n');
        Dictionary<string, int> columnIndex = new();

        string[] headers = splitUp[0].Split('\t');
        for (int i = 0; i<headers.Length; i++)
            columnIndex[headers[i].Trim()] = i;

        List<T> toReturn = new();
        for (int i = 1; i<splitUp.Length; i++)
        {
            T nextData = new();
            toReturn.Add(nextData);
            string[] thisRow = splitUp[i].Split('\t');

            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (columnIndex.TryGetValue(field.Name, out int index))
                {
                    string sheetValue = thisRow[index].Trim();
                    if (field.FieldType == typeof(int))
                        field.SetValue(nextData, StringToInt(sheetValue));
                    else if (field.FieldType == typeof(bool))
                        field.SetValue(nextData, StringToBool(sheetValue));
                    else if (field.FieldType == typeof(string))
                        field.SetValue(nextData, sheetValue);

                    int StringToInt(string line)
                    {
                        try
                        {
                            return (line.Equals("")) ? -1 : int.Parse(line);
                        }
                        catch (FormatException)
                        {
                            return 0;
                        }
                    }

                    bool StringToBool(string line)
                    {
                        return line.Equals("TRUE");
                    }
                }
                else
                {
                    if (field.FieldType == typeof(Sprite))
                        field.SetValue(nextData, Resources.Load<Sprite>($"Card Art/{thisRow[columnIndex["cardName"]]}"));
                }
            }

        }
        return toReturn;
    }
}
