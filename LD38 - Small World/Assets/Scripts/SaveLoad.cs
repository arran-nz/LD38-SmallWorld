using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager{

	static string highscoreExt = "scores.roo";


	// ==============================================================================
	// HIGH SCORE DATA
	// ==============================================================================

	public static void SaveScore(float[] score_P)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream stream = new FileStream(Application.persistentDataPath + "/" + highscoreExt, FileMode.Create); // FILE PATH
		ScoreData data = new ScoreData(score_P); // CREATE NEW DATA CLASS
		bf.Serialize(stream, data); // SAVE DATA
		stream.Close(); // CLOSE DATA STREAM
	}

	public static float[] LoadScore()
	{
		if(File.Exists(Application.persistentDataPath + "/" + highscoreExt)) // IF FILE IS THERE
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/" + highscoreExt, FileMode.Open); // FILE PATH
			ScoreData data = bf.Deserialize(stream) as ScoreData; // LOAD AS CORRECT TYPE
			stream.Close();// CLOSE DATA  STREAM

			return data.scores; // RETURN LOADED DATA
		}
		else
		{
			return new float[2]; // RETURN 0
		}
	}
		

[Serializable]
public class ScoreData {

		public float[] scores;
		public ScoreData(float[] scores_P)
	{
			scores = new float[2];
			scores[0] = scores_P[0];
			scores[1] = scores_P[1];
	}
}

}		