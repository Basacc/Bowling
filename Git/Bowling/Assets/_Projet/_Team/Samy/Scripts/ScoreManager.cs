using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public List<GameObject> targets = new List<GameObject>();
    private List<GameObject> fallenQuilles = new List<GameObject>();
    public Text score;
    int scoreValue = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
            StartCoroutine("CountScore");
    }

    IEnumerator CountScore()
    {
        if (targets == null)
            targets.AddRange(GameObject.FindGameObjectsWithTag("Quille"));

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

        scoreValue += fallenQuilles.Count;
        score.text = scoreValue.ToString();
        fallenQuilles.Clear();
        targets.Clear();

    }
}



