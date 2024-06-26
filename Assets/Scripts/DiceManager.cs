using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public List<DiceBehaviour> dices = new List<DiceBehaviour>();
    public List<DiceBehaviour> lastdices = new List<DiceBehaviour>();
    public List<int> results = new List<int>();

    public SummCalc summCalc;
    public static DiceManager diceManager;
    UISystem uiSystem;
    float t;
    public float waitTime;
    bool waitforresult;
 
    private void Awake()
    {
        diceManager = this;
        uiSystem=FindObjectOfType<UISystem>();
        t = waitTime;
        /*DiceBehaviour[] godices = FindObjectsOfType<DiceBehaviour>();

        foreach (DiceBehaviour godice in godices)
        {
            dices.Add(godice.GetComponent<DiceBehaviour>());
        }*/
    }
    public void Throw()
    {
        lastdices.Clear();
        results.Clear();
        foreach (DiceBehaviour dice in dices)
        {
            dice.GetComponent<DiceBehaviour>().ResetCube();
        }
        t = 0;
        waitforresult=true;
    }

    public void CheckResult(int r)
    {
        results.Add(r);
        if (t < waitTime&& waitforresult)
        {
            if (results.Count == dices.Count) 
            {
                preSendResult();
            }
            Debug.Log(results.Count +" " +dices.Count);
        } 
    }
    public void preSendResult()
    {
        waitforresult = false;
        foreach (DiceBehaviour lastdice in lastdices)
        {
            lastdice.trick.Invoke();
        }
        SendResult();
    }
    public void SendResult()
    {

        Dictionary<int, int> valueCount = new Dictionary<int, int>();

        foreach (int value in results)
        {
            if (valueCount.ContainsKey(value))
            {
                valueCount[value]++;
            }
            else
            {
                valueCount.Add(value, 1);
            }
        }

        List<Vector2Int> sortedValues = new List<Vector2Int>(valueCount.Count);

        foreach (KeyValuePair<int, int> pair in valueCount)
        {
            sortedValues.Add(new Vector2Int(pair.Key, pair.Value));
        }

        sortedValues.Sort((a, b) => b.x.CompareTo(a.x));

        summCalc.CheckCombinations(sortedValues);
    }
    public void Update()
    {
        if (waitforresult)
        {
            t += Time.deltaTime;
            if (t >= waitTime) 
            {
                preSendResult();
            }
        }
        
    }
}
