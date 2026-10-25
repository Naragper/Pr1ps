using UnityEngine;
using static System.MathF;

static public class Utils {

	// Exponential decay
	static public Vector3 ExpDecayV3(Vector3 a, Vector3 b, float decay, float t) => b + (a - b) * Exp(-decay * t);
	static public Vector2 ExpDecayV2(Vector2 a, Vector2 b, float decay, float t) => b + (a - b) * Exp(-decay * t);
	
	static public Vector2 ExpDecayV2Eps(Vector2 a, Vector2 b, float decay, float t, float epsilon = 0.02f) {
		Vector2 result = b + (a - b) * Exp(-decay * t);
		return (result.sqrMagnitude < epsilon) ? Vector2.zero : result;
	}

	// Time formatting
	static public string GetFormattedTimeFromInt(int ms) {
			int min = ms / 60000; 
			int rem = ms % 60000;
			int sec = rem / 1000;
			int mil = rem % 1000;
			return $"{min}:{sec:D2}:{mil:D3}";
	}

	static public int GetIntFromFormattedTime(string formattedTime) {
		// String format: "m:s:ms" or "s:ms"
		int timeInMilliseconds = 0;
		string[] sarray = formattedTime.Split(":");
		for (int i = 0; i < sarray.Length; i++) {
			int n = int.Parse(sarray[^(i + 1)]);
			if (i == 2) n *= 60000;  // minutes
			if (i == 1) n *= 1000; 	 // seconds
			timeInMilliseconds += n; // milliseconds
		};
		return timeInMilliseconds;
	}

}
