using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerWithClass : MonoBehaviour
{

    public List<GameObject> targets = new List<GameObject>();
    private List<GameObject> fallenQuilles = new List<GameObject>();

    private List<Turn> turnHistoric = new List<Turn>(10);
    private int turn = 0;

    public Text score;
    int scoreValue = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
            StartCoroutine("CountScore");
    }

    IEnumerator CountScore()
    {
        InitList(targets);

        yield return new WaitForSeconds(5);

        //Check fallen quilles
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

        //Count score
        switch (turn)
        {
            //Last turn
            case 9:
                break;
            //All other turns
            default:
                //First throw
                Debug.Log("Premier lancer?: " + turnHistoric[turn].firstThrow + " - FallenQuille:" + fallenQuilles.Count);

                if (turnHistoric[turn].firstThrow)
                {
                    //Init score
                    turnHistoric[turn].scoreFirstThrow = fallenQuilles.Count;
                    turnHistoric[turn].scoreTurn = turnHistoric[turn].scoreFirstThrow;
                    scoreValue += turnHistoric[turn].scoreFirstThrow;
                    fallenQuilles.Clear();


                    //Check if Strike or Spare previous turn and init score
                    if (turnHistoric[turn - 1] != null)
                    {
                        if (turnHistoric[turn - 1].strike || turnHistoric[turn - 1].spare)
                        {
                            Debug.Log("Strike or spare previous turn detected");
                            turnHistoric[turn - 1].scoreTurn += turnHistoric[turn].scoreFirstThrow;
                            scoreValue += turnHistoric[turn].scoreFirstThrow;
                        }
                    }

                    //Check Strike this turn
                    if (turnHistoric[turn].scoreFirstThrow == 10)
                    {
                        Debug.Log("Strike!");
                        turnHistoric[turn].strike = true;
                        turn++;
                    }
                    else
                    {
                        turnHistoric[turn].firstThrow = false; //?passage dans le else suivant?
                    }
                }
                //Second throw
                else
                {
                    //Init score
                    turnHistoric[turn].scoreSecondThrow = fallenQuilles.Count;
                    turnHistoric[turn].scoreTurn += turnHistoric[turn].scoreFirstThrow;
                    scoreValue += turnHistoric[turn].scoreSecondThrow;
                    fallenQuilles.Clear();


                    //Check if Strike previous turn and init score
                    if (turnHistoric[turn - 1] != null)
                    {
                        if (turnHistoric[turn - 1].strike)
                        {
                            Debug.Log("Strike previous turn detected");
                            turnHistoric[turn - 1].scoreTurn += turnHistoric[turn].scoreSecondThrow;
                            scoreValue += turnHistoric[turn].scoreSecondThrow;
                        }
                    }

                    //Check Spare this turn
                    if (turnHistoric[turn].scoreFirstThrow + turnHistoric[turn].scoreSecondThrow == 10)
                    {
                        Debug.Log("Spare!");
                        turnHistoric[turn].spare = true;
                    }
                    turn++;
                }
                break;
        }

        score.text = scoreValue.ToString();
        targets.Clear();
    }

    private void InitList(List<GameObject> listToInit)
    {
        if (listToInit.Count == 0)
            listToInit.AddRange(GameObject.FindGameObjectsWithTag("Quille"));
    }
}

public class Turn
{
    public int scoreFirstThrow;
    public int scoreSecondThrow;
    public int scoreTurn;
    public bool spare = false;
    public bool strike = false;
    public bool firstThrow = true;
    public bool extraThrow = false;

}



