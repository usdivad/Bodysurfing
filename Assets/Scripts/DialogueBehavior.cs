﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Csv;

public class DialogueBehavior : MonoBehaviour {
	public string pathToDialogueCsv;
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
		List<List<string>> data = CsvFileReader.ReadAll (path, System.Text.Encoding.GetEncoding ("utf-16"));
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
	string GetDialogueForCharacter(int playerIndex, int speakerIndex)
	{
		List<string> dialogueRow = this.dialogueData [playerIndex];
		string dialogueLine = dialogueRow [INTERACTION_INDEX_OFFSET + speakerIndex];
		return dialogueLine;
	}
}
