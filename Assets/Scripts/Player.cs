using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player{
	public string name;
	public List<Piece> pieces;
	public Player(string name, List<Piece> pieces){
		this.name = name;
		this.pieces = pieces;
	}
}
