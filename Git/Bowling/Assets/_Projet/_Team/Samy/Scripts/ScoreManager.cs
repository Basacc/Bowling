using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public GameObject[] targets;
    private List<GameObject> fallenQuilles = new List<GameObject>();
    private List<Vector3> initPos = new List<Vector3>();

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
            StartCoroutine("CountScore");
    }

    IEnumerator CountScore()
    {
        if (targets != null)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                initPos.Add(targets[i].transform.position);
            }

        }
        yield return new WaitForSeconds(3);
        for (int i = 0; i < targets.Length; i++)
        {
            if (initPos[i] != targets[i].transform.position)
            {
                fallenQuilles.Add(targets[i]);
                Destroy(targets[i]);
            }
        }
    }
}



