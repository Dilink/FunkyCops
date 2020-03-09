﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Ma_MainMenuManager : MonoBehaviour
{
    public static Ma_MainMenuManager Instance;

    public List<Sc_CharacterParameters> selectedCharacters;
    public Transform[] selectedCharactersEmplacements;
    public GameObject[] selectedEmplacementsGameobjects = new GameObject[3];
    public bool[] selectedEmplacementsStatus = new bool[3];

    [Header("Navigation elements")]

    public RectTransform currentScreen;
    [Space]
    public RectTransform MainMenuRect;
    public RectTransform OptionsRect;
    public RectTransform LevelRect;
    public RectTransform CharacterRect;
    public RectTransform ItemsUpgradeRect;

    [Header("MainMenu elements")]

    [Header("Options elements")]

    [Header("Levels elements")]

    [Header("Items elements")]

    [Header("Team Builder elements")]
    public GridLayoutGroup cardsGrid;
    public RectTransform characterEmplacementOne;
    public RectTransform characterEmplacementTwo;
    public RectTransform characterEmplacementThree;

    [Header("Currency bar elements")]
    public TMP_Text currencyText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentScreen = MainMenuRect;
    }

    // -------------
    // NAVIGATION
    // -------------
    #region navigation
    // MAIN MENU
    public void DisplayMainMenuScreen(bool fromLeft)
    {
        if (fromLeft)
        {
            currentScreen.DOAnchorPos3DX(2000, 0.6f, false);
        }
        else
        {
            currentScreen.DOAnchorPos3DX(-2000, 0.6f, false);

        }

        currentScreen = MainMenuRect;

        if (fromLeft)
        {
            currentScreen.DOAnchorPos3DX(-2000, 0, false);
        }
        else
        {
            currentScreen.DOAnchorPos3DX(2000, 0, false);
        }

        currentScreen.DOAnchorPos3DX(0, 0.6f, false);
    }


    // OPTIONS
    public void DisplayOptionsScreen(bool fromLeft)
    {
        if (fromLeft)
        {
            currentScreen.DOAnchorPos3DX(2000, 0.6f, false);
        }
        else
        {
            currentScreen.DOAnchorPos3DX(-2000, 0.6f, false);
        }

        currentScreen = OptionsRect;

        if (fromLeft)
        {
            currentScreen.DOAnchorPos3DX(-2000, 0, false);
        }
        else
        {
            currentScreen.DOAnchorPos3DX(2000, 0, false);
        }

        currentScreen.DOAnchorPos3DX(0, 0.6f, false);
    }


    // LEVEL
    public void DisplayLevelScreen(bool fromLeft)
    {
        if (fromLeft)
        {
            currentScreen.DOAnchorPos3DX(2000, 0.6f, false);
        }
        else
        {
            currentScreen.DOAnchorPos3DX(-2000, 0.6f, false);
        }

        currentScreen = LevelRect;

        if (fromLeft)
        {
            currentScreen.DOAnchorPos3DX(-2000, 0, false);
        }
        else
        {
            currentScreen.DOAnchorPos3DX(2000, 0, false);
        }

        currentScreen.DOAnchorPos3DX(0, 0.6f, false);
    }


    // CHARACTERS
    public void DisplayCharacterScreen(bool fromLeft)
    {
        if (fromLeft)
        {
            currentScreen.DOAnchorPos3DX(2000, 0.6f, false);
        }
        else
        {
            currentScreen.DOAnchorPos3DX(-2000, 0.6f, false);

        }

        currentScreen = CharacterRect;

        if (fromLeft)
        {
            currentScreen.DOAnchorPos3DX(-2000, 0, false);
        }
        else
        {
            currentScreen.DOAnchorPos3DX(2000, 0, false);

        }

        currentScreen.DOAnchorPos3DX(0, 0.6f, false);
    }

    public void DisplayItemsUpgradeScreen()
    {
        ItemsUpgradeRect.DOAnchorPos3DY(0, 0.6f, false);
    }

    public void HideItemsDisplayScreen()
    {
        ItemsUpgradeRect.DOAnchorPos3DY(1200, 0.6f, false);
    }

    #endregion
    // -------------
    // OPTIONS
    // -------------
    #region options

    #endregion
    // -------------
    // LEVELS
    // -------------
    #region levels

    #endregion
    // -------------
    // ITEMS UPGRADE
    // -------------
    #region items

    #endregion
    // -------------
    // TEAM BUILDER
    // -------------
    #region teambuilder
    // Add the card to the selected characters zone
    public void AddCardToSelectedEmplacements(GameObject card, int emplacement)
    {
        card.transform.DOMove(selectedCharactersEmplacements[emplacement].position, 0.2f, false);
        selectedEmplacementsStatus[emplacement] = true;
        selectedEmplacementsGameobjects[emplacement] = card;
    }

    // Remove the card to the selected characters zone
    public void RemoveCardFromSelectedEmplacement(GameObject card)
    {
        for(int i =0; i< selectedEmplacementsGameobjects.Length; i++)
        {
            if(selectedEmplacementsGameobjects[i] == card)
            {
                selectedEmplacementsStatus[i] = false;
                selectedEmplacementsGameobjects[i] = null;
            }
        }
    }

    // Return all the scriptables objects of the cards in selected characters zone
    public void GetSelectedPlayerScriptablesList()
    {
        for(int k = 0; k < selectedEmplacementsGameobjects.Length; k++)
        {
            if(selectedEmplacementsGameobjects[k] != null)
                selectedCharacters.Add(selectedEmplacementsGameobjects[k].GetComponent<Sc_TeamBuilderPlayerCards>().characterParameters);
        }
    }

    public bool IsCharacterSelectionFull()
    {
        for(int i = 0; i < selectedEmplacementsStatus.Length; i++)
        {
            if (!selectedEmplacementsStatus[i])
            {
                return true;
            }
        }
        return false;
    }

    public void BrowsePlayerCardsLeft()
    {

    }

    public void BrowsePlayerCardsRight()
    {

    }
    #endregion
    // -------------
    // CURRENCY BAR
    // -------------
    #region currency

    public void UpdateCurrencyBar(float value)
    {
        currencyText.text = value.ToString();
    }

    #endregion
}
