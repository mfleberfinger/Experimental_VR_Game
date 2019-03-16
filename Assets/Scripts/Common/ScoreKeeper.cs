using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton for keeping track of players' scores.
/// </summary>
public class ScoreKeeper
{
	private List<KeyValuePair<string, int>> m_scoreList;
    private static ScoreKeeper m_inst = null; // singleton instance

    public static ScoreKeeper instance()
    {
        if (m_inst == null) m_inst = new ScoreKeeper();
        return m_inst;
    }

    private ScoreKeeper()
    {
		m_scoreList = new List<KeyValuePair<string, int>>();
    }

	/// <summary>
	/// Get a list of names and scores, in descending order by score.
	/// </summary>
	/// <returns>Array of (name, score) strings in order by score.</returns>
    public KeyValuePair<string, int>[] GetScores()
	{
		return m_scoreList.ToArray();
	}

	/// <summary>
	/// Add a name and score to the high score list, if the score is high
	/// enough. Keeps the list in descending order by score. Saves the list.
	/// </summary>
	/// <param name="name">Name to associate with the score.</param>
	/// <param name="score">The score to add to the list.</param>
	public void SetScore(string name, int score)
	{
		m_scoreList.Add(new KeyValuePair<string, int>(name, score));
	}

	/// <summary>
	/// Defines which of two KeyValuePairs should be considered greater for the
	/// purpose of sorting scores.
	/// </summary>
	/// <param name="score1"></param>
	/// <param name="score2"></param>
	private int ScoreComparer(KeyValuePair<string, int> score1,
		KeyValuePair<string, int> score2)
	{
		// See MSDN Comparison<T> Delegate documention for explanation.
		// Return codes...
		// Less than 0:		score1 < score2
		// 0:				score1 == score2
		// Greater than 0:	score1 > score2
		return score1.Value - score2.Value;
	}
}
