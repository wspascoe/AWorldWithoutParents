using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Image displayImage;

    private void Start()
    {
        ResetDisplay();
    }

    public void Display(string dialog)
    {
        dialogText.text = dialog;
        displayImage.gameObject.SetActive(true);
    }

    public void ResetDisplay()
    {
        displayImage.gameObject.SetActive(false);
        dialogText.text = "";
        
    }
}
