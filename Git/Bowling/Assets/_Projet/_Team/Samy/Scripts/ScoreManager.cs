﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public List<GameObject> targets = new List<GameObject>();
    private List<GameObject> fallenQuilles = new List<GameObject>();
    public Text score;
    int scoreValue = 0;

    private bool strike = false; //true if strike previous turn
    private bool spare = false; //true if spare previous turn
    private bool firstTurn = true; //true if first turn

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
            StartCoroutine("CountScore");
    }

    IEnumerator CountScore()
    {
        InitList(targets);

        //Check fallen quilles
        yield return new WaitForSeconds(5);
        for (int i = 0; i < targets.Count; i++)
        {
            Quaternion quillRot = targets[i].transform.rotation;
            float angleFromTop = Quaternion.Angle(Quaternion.Euler(new Vector3(-90, 0, 0)), quillRot);

            if (angleFromTop > 40)
            {
                fallenQuilles.Add(targets[i]);
                Destroy(targets[i]);
            }
        }

        SetSpareAndStrike(fallenQuilles.Count, firstTurn);

        //Count score
        if (firstTurn)
        {
            if (spare || strike)
            {
                scoreValue += fallenQuilles.Count * 2;
            }
            else
            {
                scoreValue += fallenQuilles.Count;
            }
        }
        else
        {
            if (strike)
            {
                scoreValue += fallenQuilles.Count * 2;
            }
            else
            {
                scoreValue += fallenQuilles.Count;
            }
        }

        scoreValue += fallenQuilles.Count;
        score.text = scoreValue.ToString();

        fallenQuilles.Clear();

        targets.Clear();
    }

    private void InitList(List<GameObject> listToInit)
    {
        if (listToInit.Count == 0)
            listToInit.AddRange(GameObject.FindGameObjectsWithTag("Quille"));
    }

    private void SetSpareAndStrike(int fallenQuilles, bool firstTurn)
    {
        if (fallenQuilles >= 10)
            if (firstTurn)
                strike = true;
            else
                spare = true;
        else
        {
            strike = false;
            spare = false;
        }
    }
}



