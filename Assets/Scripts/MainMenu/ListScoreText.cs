using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ListScoreText : MonoBehaviour
{
    Text m_scoreText = null;
    // Start is called before the first frame update
    void Start()
    {
        m_scoreText = GetComponent<Text>();

        //TODO: delete next two lines when score is populated
        ScoreKeeper.instance().SetScore("who", 20);
        ScoreKeeper.instance().SetScore("bastank", 99);

        foreach (var pair in ScoreKeeper.instance().GetScores())
        {
            m_scoreText.text += string.Format("{0} : {1}\n", pair.Key, pair.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
