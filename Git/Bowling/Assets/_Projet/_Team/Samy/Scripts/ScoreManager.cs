using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public List<GameObject> targets = new List<GameObject>();
    private List<GameObject> fallenQuillesT1 = new List<GameObject>();
    private List<GameObject> fallenQuillesT2 = new List<GameObject>();
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
                if (firstTurn)
                    fallenQuillesT1.Add(targets[i]);
                else
                    fallenQuillesT2.Add(targets[i]);
                Destroy(targets[i]);
            }
        }

        //Count score
        if (firstTurn)
        {
            if (spare || strike)
            {
                scoreValue += fallenQuillesT1.Count * 2;
            }
            else
            {
                scoreValue += fallenQuillesT1.Count;
            }
        }
        else
        {
            if (strike)
            {
                scoreValue += fallenQuillesT2.Count * 2;
            }
            else
            {
                scoreValue += fallenQuillesT2.Count;
            }
        }

        SetStrike(fallenQuillesT1.Count, firstTurn);
        SetSpare(fallenQuillesT1.Count, fallenQuillesT2.Count, firstTurn);

        if (!firstTurn)
        {
            fallenQuillesT1.Clear();
            fallenQuillesT2.Clear();
        }

        if (!strike)
        {
            firstTurn = false;
        }
        else
        {
            firstTurn = true;
            fallenQuillesT1.Clear();
        }
        score.text = scoreValue.ToString();
        targets.Clear();
    }



    private void InitList(List<GameObject> listToInit)
    {
        if (listToInit.Count == 0)
            listToInit.AddRange(GameObject.FindGameObjectsWithTag("Quille"));
    }

    private void SetStrike(int fallenQuilles, bool firstTurn)
    {
        if (firstTurn && fallenQuilles == 10)
        {
                strike = true;
            Debug.Log("strike");
        }

        if(firstTurn == false)
        {
            strike = false;
            Debug.Log("strike end");
        }


    }

    private void SetSpare(int countT1, int countT2, bool firstTurn)
    {
        if (firstTurn == false && !strike && (countT1 + countT2) == 10)
        {
                spare = true;
            Debug.Log("spare");
        }
       
        if(strike || firstTurn)
        {
            spare = false;
            Debug.Log("spare end");
        }


    }
}



