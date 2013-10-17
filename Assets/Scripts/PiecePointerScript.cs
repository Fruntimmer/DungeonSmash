using UnityEngine;
using System.Collections;

public class PiecePointerScript : MonoBehaviour {
	public Piece piece;

	void FixedUpdate() {
		piece.FixedUpdate();
	}	
}
