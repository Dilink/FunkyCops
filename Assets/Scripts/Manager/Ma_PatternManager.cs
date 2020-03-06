﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class Ma_PatternManager : MonoBehaviour
{
    [ReadOnly]
    public List<Sc_Pattern> currentPatternsList = new List<Sc_Pattern>();
    [ReadOnly]
    public List<Sc_Pattern> availablePatternList = new List<Sc_Pattern>();
    [ReadOnly]
    public List<Sc_Pattern> patternsForCancellation = new List<Sc_Pattern>();

    [ReadOnly]
    public Sc_Pattern futurePattern;
    public readonly int patternCount = 5;

    private static System.Random rand = new System.Random();

    private void Awake()
    {
        LoadAvailablePatterns();
        GenerateStartPattern();
        OnTurnStart();
    }

    private void OnTurnStart()
    {


        patternsForCancellation.Clear();

        for (int j = 0; j < currentPatternsList.Count; j++)
        {
            UpdateCancelMarker(j);
        }

        var level = GameManager.Instance.levelConfig;
        int count = rand.Next(level.minPatternsToCancelAttack, level.maxPatternsToCancelAttack);
        var copy = new List<Sc_Pattern>(currentPatternsList);
        RandomizeList(ref copy);
        var queue = new Queue<Sc_Pattern>(copy);

        for (int i = 0; i < count; i++)
        {
            var pattern = queue.Dequeue();
            UpdateCancelMarker(currentPatternsList.IndexOf(pattern));
            patternsForCancellation.Add(pattern);
        }

        for (int j = 0; j < currentPatternsList.Count; j++)
        {
            UpdateCancelMarker(j);
        }
    }

    private void UpdateCancelMarker(int index)
    {
        bool flag = patternsForCancellation.Contains(currentPatternsList[index]);
        GameManager.Instance.uiManager.UpdateCancelMarkerIcon(index, flag);
    }

    public void OnTurnEnd(bool isLevelFinished = false, bool ignoreDamages = false)
    {
        if (!ignoreDamages && patternsForCancellation.Count() != 0)
        {
            GameManager.Instance.FunkVariation(GameManager.Instance.funkDamagesToDeal());
        }

        if (!isLevelFinished)
        {
            OnTurnStart();
        }
    }

    private void LoadAvailablePatterns()
    {
        availablePatternList.Clear();

        // Load all assets of type Sc_Pattern that are located in Assets/Patterns folder
        string[] guids2 = AssetDatabase.FindAssets("t:Sc_Pattern", new[] { "Assets/Patterns/Player" });
        foreach (var i in guids2)
        {
            string path = AssetDatabase.GUIDToAssetPath(i);
            Sc_Pattern pattern = AssetDatabase.LoadAssetAtPath<Sc_Pattern>(path);

            pattern.Name = Path.GetFileNameWithoutExtension(path);
            pattern.Category = Directory.GetParent(path).Name;

            availablePatternList.Add(pattern);
        }

        RandomizeList(ref availablePatternList);
    }

    private void RandomizeList(ref List<Sc_Pattern> list)
    {
        list = list.OrderBy(x => rand.Next()).ToList();
    }

    /*private Tuple<int, int> getSizeOfPattern(Sc_Pattern pattern)
    {
        int minW = int.MaxValue;
        int maxW = int.MinValue;
        int minH = int.MaxValue;
        int maxH = int.MinValue;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (pattern.Matrix[i, j])
                {
                    minW = Math.Min(minW, i + 1);
                    maxW = Math.Max(maxW, i + 1);

                    minH = Math.Min(minH, j + 1);
                    maxH = Math.Max(maxH, j + 1);
                }
            }
        }

        return new Tuple<int, int>(maxW - minW + 1, maxH - minH + 1);
    }*/

    public void CheckGridForPatternAndReact(float multiplier)
    {
        var res = JustCheckGridForPattern();
        if (res.HasValue)
        {
            patternsForCancellation.Remove(res.Value.Item2);
            UpdateCancelMarker(res.Value.Item1);
            GameManager.Instance.OnPatternResolved(res.Value.Item1, multiplier);
            return;
        }
    }

    private Optional<Tuple<int, Sc_Pattern>> JustCheckGridForPattern()
    {
        // take scene grid and check each pattern if currentPatternsList if it matches
        for (int i = 0; i < currentPatternsList.Count(); i++)
        {
            Sc_Pattern pattern = currentPatternsList[i];
            if (PatternValidation(GameManager.Instance.allTiles, pattern))
            {
                return new Optional<Tuple<int, Sc_Pattern>>(new Tuple<int, Sc_Pattern>(i, pattern));
            }
        }
        return new Optional<Tuple<int, Sc_Pattern>>();
    }

    private Mb_Tile[] GetAllTileWithPlayer(Mb_Tile[] allTiles)
    {
        List<Mb_Tile> tiles = new List<Mb_Tile>();

        for (int i = 0; i < allTiles.Length; i++)
        {
            if (allTiles[i].playerOnTile)
            {
                tiles.Add(allTiles[i]);
            }
        }

        // Sort tiles by position
        return tiles.OrderBy(o => o.posX).ThenBy(o => o.posZ).ToArray();
    }

    bool PatternValidation(Mb_Tile[] allTiles, Sc_Pattern pattern)
    {
        Mb_Tile[] playerTiles = GetAllTileWithPlayer(allTiles);
        int[] patternKeyPointsIndices = pattern.Matrix.GetTrueValuesIndices().OrderBy(i => pattern.Matrix.GetLocation(i).x).ThenBy(i => pattern.Matrix.GetLocation(i).y).ToArray();

        // Check for keypoints distance in patterns
        bool flagX1 = playerTiles[0].posX - playerTiles[1].posX == pattern.Matrix.GetLocation(patternKeyPointsIndices[0]).x - pattern.Matrix.GetLocation(patternKeyPointsIndices[1]).x;
        bool flagX2 = playerTiles[0].posX - playerTiles[2].posX == pattern.Matrix.GetLocation(patternKeyPointsIndices[0]).x - pattern.Matrix.GetLocation(patternKeyPointsIndices[2]).x;

        bool flagZ1 = playerTiles[0].posZ - playerTiles[1].posZ == pattern.Matrix.GetLocation(patternKeyPointsIndices[0]).y - pattern.Matrix.GetLocation(patternKeyPointsIndices[1]).y;
        bool flagZ2 = playerTiles[0].posZ - playerTiles[2].posZ == pattern.Matrix.GetLocation(patternKeyPointsIndices[0]).y - pattern.Matrix.GetLocation(patternKeyPointsIndices[2]).y;

        return flagX1 && flagX2 && flagZ1 && flagZ2;
    }

    void GenerateStartPattern()
    {
        /*for (int i = 0; i < 5; i++)
        {
            while (true)
            {
                var pattern = GetRandomPatternDifferentOfCurrents();
                var check = PatternValidation(GameManager.Instance.allTiles, pattern);
                if (!check)
                {
                    currentPatternsList.Add(pattern);
                    break;
                }
            }
        }

        // Legacy way to add start patterns
        /*for (int i = 0; i < 5; i++)
        {
            currentPatternsList.Add(GetRandomPatternDifferentOfCurrents());
        }*

        while (true)
        {
            var pattern = GetRandomPatternDifferentOfCurrents();
            var check = PatternValidation(GameManager.Instance.allTiles, pattern);
            if (!check)
            {
                futurePattern = pattern;
                break;
            }
        }*/



        for (int i = 0; i < 5; i++)
        {
            currentPatternsList.Add(PickPattern());
        }
        futurePattern = PickPattern();

        for (int i = 0; i < currentPatternsList.Count(); i++)
        {
            Sc_Pattern pattern = currentPatternsList[i];
            GameManager.Instance.uiManager.UpdatePatternsBarIcon(i, pattern);
        }
        GameManager.Instance.uiManager.UpdatePatternsBarIcon(currentPatternsList.Count(), futurePattern);
    }

    /*private Sc_Pattern GetRandomPatternDifferentOfCurrents()
    {
        List<Sc_Pattern> backupCurrentPatternsList = new List<Sc_Pattern>();
        List<Sc_Pattern> result = null;
        while (true)
        {
            // ADDITIONER TOUT LES POIDS POSSIBLE
            int total = 0;
            Dictionary<string, Tuple<int, List<Sc_Pattern>>> dic = new Dictionary<string, Tuple<int, List<Sc_Pattern>>>();
            foreach (var cat in GameManager.Instance.levelConfig.patternCategories)
            {
                var lis = availablePatternList.Where(e => e.Category == cat.Key).ToList();
                var tup = new Tuple<int, List<Sc_Pattern>>(total + cat.Value - 1, lis);
                dic.Add(cat.Key, tup);
                total += cat.Value;
            }

            // TIRER UNE RANDOM ENTRE 0 ET CE POID
            total -= 1;

            int pond = rand.Next(total);

            if (result != null)
                result.Clear();
            else
                result = new List<Sc_Pattern>();

            // CHECKER LE DICT EN ATTENDANT QUE LE TOTAL SOIT < A LA VALEUR DE POID CUMULE
            foreach (var entry in GameManager.Instance.levelConfig.patternCategories)
            {
                pond -= entry.Value;
                if (pond <= 0)
                {
                    result = dic[entry.Key].Item2;
                    break;
                }
                              
            }

            // RETIRER LES PATTERNS DES PATTERNS PRESENTS ET DE LA PREVIEW DE LA LISTE POSSIBLE
            foreach (Sc_Pattern item in currentPatternsList)
            {
                result.Remove(item);
                backupCurrentPatternsList.Add(item);
            }
            result.Remove(futurePattern);

            if (result.Count() != 0)
            {
                break;
            }
        }
        
        // TIRER UNE RANDOM DANS LA LISTE QUI A ARRETE LA VALEUR DE POID
        Sc_Pattern stock = result[rand.Next( result.Count())];

        // REMETTRE LES PATTERNS QU ON VIENT DE RETIRER
        result.AddRange(backupCurrentPatternsList);

        return stock;
    }*/

    public void RotatePattern(int indexInList)
    {
        currentPatternsList.RemoveAt(indexInList);
        currentPatternsList.Add(futurePattern);

        UpdateCancelMarker(indexInList);

        for (int i = indexInList+1; i < currentPatternsList.Count()+1; i++)
        {
            GameManager.Instance.uiManager.MovePatterns(i);
        }

        if (patternsForCancellation.Contains(currentPatternsList[indexInList]))
        {
            patternsForCancellation.Remove(currentPatternsList[indexInList]);
        }

        GameManager.Instance.uiManager.RemovePattern(indexInList);

        //futurePattern = GetRandomPatternDifferentOfCurrents();
        futurePattern = PickPattern();
        GameManager.Instance.uiManager.UpdatePatternsBarIcon(currentPatternsList.Count(), futurePattern);

    }

    private Sc_Pattern PickPattern(List<PatternCategory> categories =null, List<Sc_Pattern> patterns=null)
    {
        if (categories == null)
        {
            categories = new List<PatternCategory>(GameManager.Instance.levelConfig.rounds[GameManager.Instance.currentRoundCountFinished].patternCategories);
        }

        if (patterns == null)
        {
            patterns = new List<Sc_Pattern>(availablePatternList);
        }

        // Choose a random category based on weight
        var cat = ChooseWeightedRandomization(categories);

        // If there are no category left
        if (!cat.HasValue)
        {
            return null;
        }

        // Get all patterns in that category
        var list2 = patterns.Where(e => e.Category == cat.Value.Name).ToList();

        // If there aren't no patterns left
        if (list2.Count() == 0)
        {
            // Remove the category from the list
            categories.Remove(cat.Value);
            // Try to pick a pattern from another category with no prepared patterns list
            return PickPattern(categories, null);
        }
        // If there is at least a pattern left in the list
        else
        {
            // Pick a random pattern in the list
            var pattern = list2[rand.Next(list2.Count())];
            // If pattern is already in current pattern to play or
            // if pattern is the future pattern or
            // if pattern is already validated with no move by the players
            if (currentPatternsList.Contains(pattern) || (pattern == futurePattern) || PatternValidation(GameManager.Instance.allTiles, pattern))
            {
                // Remove the pattern from the prepared patterns list
                list2.Remove(pattern);

                // If there are no pattern left
                if (list2.Count() == 0)
                {
                    // Remove the category
                    categories.Remove(cat.Value);
                    // Pick a new pattern from another category
                    return PickPattern(categories, null);
                }
                else
                {
                    // Pick a new pattern from the same category
                    return PickPattern(categories, list2);
                }
            }
            else
            {
                // Return the valid pattern
                return pattern;
            }
        }
    }

    private Optional<PatternCategory> ChooseWeightedRandomization(List<PatternCategory> dic)
    {
        if (dic.Count() == 0)
        {
            return new Optional<PatternCategory>();
        }

        int totalweight = dic.Sum(c => c.Weight);
        int choice = rand.Next(totalweight);
        int sum = 0;

        foreach (var obj in dic)
        {
            for (int i = sum; i < obj.Weight + sum; i++)
            {
                if (i >= choice)
                {
                    return obj;
                }
            }
            sum += obj.Weight;
        }

        return new Optional<PatternCategory>(dic.First());
    }
}