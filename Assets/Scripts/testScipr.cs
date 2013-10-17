using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testScipr : MonoBehaviour {
public Color colP1 = Color.yellow;
public Color colP2 = Color.yellow;
	// Use this for initialization
	void Start () {
		List<Piece> piecesP1 = new List<Piece>();
		//Player 1
		piecesP1.Add(new Piece(new Priest(), 10f,0f, 1, Color.red));
		piecesP1.Add(new Piece(new Warrior(), 12f,5f, 1, Color.red));
		piecesP1.Add(new Piece(new Archer(), 10,3f, 1, Color.red));
		Player p1 = new Player("p1", piecesP1);

		//Player2
		List<Piece> piecesP2 = new List<Piece>();
		piecesP2.Add(new Piece(new Warrior(), 5f,0f, 1, colP2));
		piecesP2.Add(new Piece(new Archer(), 0f,2f, 1, colP2));
		piecesP2.Add(new Piece(new Priest(), 0f,5f, 1, colP2));
		Player p2 = new Player("p2", piecesP2);

		List<Player> allPlayers = new List<Player>();
		allPlayers.Add(p1);
		allPlayers.Add(p2);

		GameScript GS = GameScript.Instance;
		GS.Start(allPlayers);
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
