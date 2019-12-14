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
    private Button next;

    [SerializeField]
    private Button previus;

    [SerializeField]
    private GameObject startPage;

    [SerializeField]
    private GameObject backPage;


    public void Start()
    {
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
            backPage.transform.localScale = new Vector3(1,1,1);
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
            startPage.transform.localScale = new Vector3(1,1,1);
        }
    }
}
