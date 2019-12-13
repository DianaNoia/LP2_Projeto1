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
    private GameObject startPage;

    [SerializeField]
    private GameObject backPage;

    public void ToggleBackPage()
    {
        backPage.SetActive(!backPage.activeInHierarchy);
    }
    public void ToggleStartPage()
    {
        startPage.SetActive(!startPage.activeInHierarchy);
    }




}
