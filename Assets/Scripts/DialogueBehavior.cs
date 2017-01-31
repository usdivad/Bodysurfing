using UnityEngine;
//using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Csv;

public class DialogueBehavior : MonoBehaviour {
	public string pathToDialogueCsv; // RELATIVE to StreamingAssets folder (so atm just the filename)
	private List<string> dialogueHeader;
	private List<List<string>> dialogueData;
	private const int INTERACTION_INDEX_OFFSET = 4;

	enum HeaderLabels {
		kCharacterIndex=0,
		kName,
		kBio,
		kMusic,
		kBPM,
		kInteractions
	}

	enum CharacterIndexes {
		kEve=0,
		kKino,
		kNara,
		kPyotr,
		kElise,
		kRaul,
		kQian
	}

	// Use this for initialization
	void Start () {
		LoadDataFromCsvPath (this.pathToDialogueCsv);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Load, parse, and format dialogue data CSV from a file path
	void LoadDataFromCsvPath(string path)
	{
		// Read CSV from path
		string fullPath = Path.Combine(Application.streamingAssetsPath, path);
		List<List<string>> data = CsvFileReader.ReadAll (fullPath, System.Text.Encoding.GetEncoding ("utf-16"));
		List<string> header = data [0];
		Debug.Log (data [0][1]);

		// Parse and format data
		data.RemoveAt (0); // Remove header row
		data.RemoveAt (0); // Remove character label row
		Debug.Log (data [0][1]);

		// Parse and format header
		header[INTERACTION_INDEX_OFFSET + 1] = "1"; // "Interactions with: 1" -> "1"; is this necessary?

		// Load into member variables
		this.dialogueData = data;
		this.dialogueHeader = header;

		//return data;
	}

	// Returns the dialogue line based on player and speakers' character indexes
	public string GetDialogueForCharacter(int playerIndex, int speakerIndex)
	{
		if (playerIndex < 0 || playerIndex >= this.dialogueData.Count ||
			speakerIndex < 0 || speakerIndex >= this.dialogueData.Count)
		{
			return "";
		}

		List<string> dialogueRow = this.dialogueData [playerIndex];
		string dialogueLine = dialogueRow [INTERACTION_INDEX_OFFSET + speakerIndex];
		return dialogueLine;
	}

	public void ConverseCharacters(int playerIndex, int speakerIndex)
	{
		string dialogueLine = this.GetDialogueForCharacter(playerIndex, speakerIndex);
		string speakerName = dialogueData [speakerIndex] [(int)HeaderLabels.kName];
		Debug.Log (speakerName.ToUpper () + ": " + dialogueLine);
		//GameObject.Find ("Dialogue Text").GetComponent<Text> ().text = dialogueLine;
	}
}
