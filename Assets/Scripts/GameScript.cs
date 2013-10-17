using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScript  {
	private static GameScript instance;

	public Player activePlayer;
	public List<Player> allPlayers = new List<Player>();
	public bool moveActive = false;
	public Piece activePiece = null;
	public List<Piece> movedPieces = new List<Piece>();
	public List<PassiveAbility> passiveAbilityToDelete = new List<PassiveAbility>();
	public GameObject cam;

	public bool isProjectileActive = false;
	public Piece projectile;

	private GameScript() {}

	public static GameScript Instance
	{	
		get 
		{
			if (instance == null)
			{
				instance = new GameScript();
			}
			return instance;
	  }
	}
	public void MoveStarted(Piece p){
		activePiece = p;
		moveActive = true;

	}
	public void MoveEnded(){
		if (isProjectileActive){
			isProjectileActive = false;
			activePlayer.pieces.Remove(projectile);
			GameObject.Destroy(projectile.geo);
			projectile = null;

		}
		else{
			movedPieces.Add(activePiece);
		}
		if (movedPieces.Count >= activePlayer.pieces.Count){
				NextPlayer();	
		}
		moveActive = false;	
		activePiece = null;
	}
	public void Start(List<Player> allPlayers){
		this.allPlayers = allPlayers;
		activePlayer = allPlayers[0];
		cam = GameObject.FindWithTag("MainCamera");
   }
	public void NextPlayer(){
	   	Debug.Log("Next Player!");
	   	movedPieces.Clear();
	   	activePlayer = allPlayers[(allPlayers.IndexOf(activePlayer)+1)%allPlayers.Count];
	   	foreach(Piece p in activePlayer.pieces){
	   		passiveAbilityToDelete.Clear();
	   		foreach (PassiveAbility ability in p.chur.passiveAbilities){
	   			if (ability.NewTurn()){
	   				passiveAbilityToDelete.Add(ability);
	   			}
			}
			foreach (PassiveAbility ability in passiveAbilityToDelete){
				p.chur.passiveAbilities.Remove(ability);
			}
		}
	}
	public bool IsPieceValid(Piece p){
		if(isProjectileActive){
			if(p == projectile)
				return true;
			else
				return false;
		}
		else if(!moveActive && activePlayer.pieces.Contains(p) && !movedPieces.Contains(p)){
   			return true;
		}
		else
   			return false;
   }
   public bool isCollisionValid(Piece p1, Piece p2){
		if(activePlayer.pieces.Contains(p1) && !activePlayer.pieces.Contains(p2) || (activePlayer.pieces.Contains(p2) && !activePlayer.pieces.Contains(p1))){
			float impact1 = Mathf.Min(1, p1.geo.rigidbody.velocity.magnitude/10);
			float impact2 = Mathf.Min(1, p2.geo.rigidbody.velocity.magnitude/10);
			float impact = Mathf.Max(impact1, impact2);
			Debug.Log(impact);
			cam.SendMessage("ActivateShake", impact);
			return true;
		}
		else
			return false;
   }
   public bool isFriendlyCollisionValid(Piece p1, Piece p2){
		if(activePlayer.pieces.Contains(p1) && activePlayer.pieces.Contains(p2))
			return true;
		else
			return false;
	}
   public void RegisterProjectile(Piece p, Piece creator){
   		isProjectileActive = true;
   		projectile = p;
   		activePlayer.pieces.Add(projectile);
   		movedPieces.Add(creator);
   }
	public void PieceDead(Piece p){
	   	for(int i = 0; i < allPlayers.Count;i++){
	   		if(allPlayers[i].pieces.Contains(p)){
	   			allPlayers[i].pieces.Remove(p);
	   			if (allPlayers[i].pieces.Count <= 0){
	   				RemovePlayer(allPlayers[i]);
	   				i--;
	   			}
   			}
		}
	}
	private void RemovePlayer(Player player){
		allPlayers.Remove(player);
		if(allPlayers.Count <= 1){
			Debug.Log(allPlayers[0] +" Wins!");
		}
	}
}