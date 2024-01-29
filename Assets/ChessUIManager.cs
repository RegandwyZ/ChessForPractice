using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChessUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _uiParent;
    [SerializeField] private TextMeshProUGUI _resultText;

    public void HideUI()
    {
        _uiParent.SetActive(false);
    }

    public void OnGameFinished(string winner)
    {
        _uiParent.SetActive(true);
        _resultText.text = $"{winner} won";
    }
}
