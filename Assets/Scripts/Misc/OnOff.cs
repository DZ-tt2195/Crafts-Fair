using UnityEngine;
using System.Collections.Generic;

public class OnOff : MonoBehaviour
{
    [SerializeField] List<GameObject> forceOn = new();
    [SerializeField] List<GameObject> forceOff = new();
    private void Start() 
    {
        foreach (GameObject next in forceOn)
            next.SetActive(true);
        foreach (GameObject next in forceOff)
            next.SetActive(false);
    }
}