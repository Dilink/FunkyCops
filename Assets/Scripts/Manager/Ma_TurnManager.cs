﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ma_TurnManager : MonoBehaviour
{
    public static Ma_TurnManager instance; // Static instance

    [Header("Turns stats")]
    [SerializeField] int MaxTurn; // Max number of turns for this level
    private int CurrentTurn = 1; // Current turn number

    private void Start()
    {
        MaxTurn = GameManager.Instance.levelConfig.rounds[0].turnLimit;
        //AI PART A CHANGER
        GameManager.Instance.aiManager.ChoosePattern();
    }

    public void BeginTurn()
    {
        /*
        // Reset all player characters move number
        for(int i =0; i < GameManager.Instance.allPlayers.Length; i++)
        {
            GameManager.Instance.allPlayers[i]..ResetMove();
        }
        */
    }

    public void EndTurn()
    {
        GameManager.Instance.UpdateFeedBackAutourGrid(0);
        GameManager.Instance.CheckGameEnd();
        GameManager.Instance.OnTurnEndPre();
        // Pass to the next turn
        if (CurrentTurn <= MaxTurn && !GameManager.Instance.isGameFinished)
        {
            CurrentTurn++;

            GameManager.Instance.uiManager.UpdateTurnsbarText(CurrentTurn, GameManager.Instance.levelConfig.rounds[0].turnLimit);

            //old deplacement System
            /*
            for (int i =0; i < GameManager.Instance.allPlayers.Length;i++)
            {
                GameManager.Instance.allPlayers[i].ResetMove();
            }*/

            GameManager.Instance.ResetMove();
            GameManager.Instance.comboManager.ResetMultiplier();

            GameManager.Instance.aiManager.ChoosePattern();

            StartCoroutine(PreventPlayerFromActing());



            GameManager.Instance.patternManager.GenerateAttackPattern();
        }
  
    }

    public IEnumerator PreventPlayerFromActing()
    {
        GameManager game = GameManager.Instance;
        game.canActForced = true;   
        game.DisableActing();
        yield return new WaitForSeconds(game.timeBetweenTurns);
        game.canActForced = false;
        game.EnableActing();
    }

    public void OnNextRound() {
        GameManager.Instance.FunkVariation(-1);
        CurrentTurn = 1;
        GameManager.Instance.uiManager.UpdateTurnsbarText(CurrentTurn, GameManager.Instance.levelConfig.rounds[0].turnLimit);

        MaxTurn = GameManager.Instance.levelConfig.rounds[GameManager.Instance.currentRoundCountFinished].turnLimit;
        GameManager.Instance.uiManager.DisplayRoundIntermediateScreen(GameManager.Instance.currentRoundCountFinished);
    }

    public bool IsLastRoundFinished()
    {
        return CurrentTurn > MaxTurn;
    }

    public int GetMaxTurn()
    {
        return MaxTurn;
    }

    public int GetCurrentTurn()
    {
        return CurrentTurn;
    }
}
