using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button searchButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button previousButton;

    [SerializeField]
    private GameObject startPage;

    [SerializeField]
    private GameObject backPage;

    [SerializeField]
    private Text[] texts;

    public bool nextWasClicked;
    public bool previousWasClicked;

    public void Start()
    {
        nextWasClicked = false;
        previousWasClicked = false;

        backPage.transform.localScale = Vector3.zero;
    }

    public void ToggleBackPage()
    {
        if (backPage.transform.localScale != Vector3.zero)
        {
            backPage.transform.localScale = Vector3.zero;
        }

        else
        {
            backPage.transform.localScale = new Vector3(1, 1, 1);
        }

    }
    public void ToggleStartPage()
    {
        if (startPage.transform.localScale != Vector3.zero)
        {
            startPage.transform.localScale = Vector3.zero;
        }

        else
        {
            startPage.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void ToggleNextEigthResults()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = "";
        }

        nextWasClicked = true;
    }
    public void TogglePreviousEigthResults()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = "";
        }

        previousWasClicked = true;
    }
}
