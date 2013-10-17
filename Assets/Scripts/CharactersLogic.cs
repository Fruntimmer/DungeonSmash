using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece{    
    public Agent chur{get; private set;}
    public GameObject geo;
    public bool isActive {get; private set;}
    private List<GameObject> collidedPieces = new List<GameObject>();
    public GameScript GS = GameScript.Instance;

    public Piece(Agent chur, float x, float z, float radius, Color col)
    {
        this.chur = chur;
        chur.parentPiece = this;
        geo = CreateGeo(x,z, radius, col);
        chur.CreateCharGeo(this);
    }
    
    private void OnCollisionEnter(Collision collision){
    	if (collision.gameObject.tag == "PieceTag" && !collidedPieces.Contains(collision.gameObject)) {
    		collidedPieces.Add(collision.gameObject);
	    	Piece colliderPiece = collision.gameObject.GetComponent<PiecePointerScript>().piece;
	    	if (isActive && GS.isCollisionValid(this, colliderPiece)) {
	    		colliderPiece.Attacked(chur);
	    	}
            if (isActive && GS.isFriendlyCollisionValid(this, colliderPiece)){
                this.chur.FriendlyCollision(colliderPiece);
            }
    	}
    }

    public void FixedUpdate() {

    	if (geo.rigidbody.velocity.magnitude < 2f && geo.rigidbody.velocity.magnitude > 0) {
    		float factor = 4 * Time.deltaTime;
    		geo.rigidbody.velocity -= new Vector3(geo.rigidbody.velocity.x * factor, 0, geo.rigidbody.velocity.z * factor);
    		if (geo.rigidbody.velocity.magnitude < 0.01f) {
    			geo.rigidbody.velocity = Vector3.zero;
    			Stopped();
    		}
    	}
    }

    public void MakeActive() {
    	isActive = true;
    	collidedPieces.Clear();
        GS.MoveStarted(this);
    }
    public void Stopped()
    {
    	if (isActive){
    		isActive = false;
            GS.MoveEnded();
    	}
    }
    public void Attacked(Agent c) {
    	chur.Attacked(c);
        if(chur.health <= 0)
        {
            GameObject.Destroy(geo);
            GS.PieceDead(this);
        }
    }
    public void Update()
    {    	
    }
    public void Move(Vector3 angle, float dist){
    	MakeActive();
    	geo.rigidbody.AddForce(angle*dist*2000);
    }
    private GameObject CreateGeo(float x,float z,float radius, Color col)
    {
        GameObject geo = (GameObject)GameObject.Instantiate((GameObject)Resources.Load("PiecePrefab"), new Vector3(x, 0, z), Quaternion.identity);
        geo.renderer.material.color = col;
        geo.transform.localScale = new Vector3(radius, 1, radius);
        geo.AddComponent("CollisionCallbackScript");
        CollisionCallbackScript geoCollisionCallbackScript = (CollisionCallbackScript)geo.GetComponent("CollisionCallbackScript");
        geoCollisionCallbackScript.collisionDelegate = delegate (Collision collision) {
            OnCollisionEnter(collision);
        };
        geo.AddComponent("PiecePointerScript");
        geo.GetComponent<PiecePointerScript>().piece = this;

        return geo;
    }
}
public class Agent
{
	public Piece parentPiece;
    public int health{ get; protected set;}
	public int damage{ get; protected set;}
	public int heal { get; protected set;}
    public List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
    public string name;

	public virtual void Attacked(Agent agent) {}
    //public virtual void Healed(Agent agent) {}
    public virtual void FriendlyCollision(Piece collided){
        foreach (PassiveAbility passive in passiveAbilities){
            passive.FriendlyCollision(collided);
        }
    }
    public virtual int GetDamage(){
        int dmg = damage;
        foreach (PassiveAbility ability in passiveAbilities)
        {
            dmg += ability.extraDmg;
        }
        Debug.Log("My damage is" + dmg);
        return dmg;
    }
    public virtual void CreateCharGeo(Piece p){}

}


public class Arrow : Agent{
	public Arrow() {
		name = "Arrow";
		damage = 2;
        health = 1;
	}
}

public class Character : Agent{
    public int mana{ get; protected set;}
    public List<Ability> abilities;
    public GameObject geo{get; protected set;}
    public string charGeo {get;protected set;}

    public override void Attacked(Agent agent){
    	health -= agent.GetDamage();
        Debug.Log("I got attacked! My health is: "+health);
    }
    public void Healed(int amount){
        health += amount;
        Debug.Log("I got healed! My health is: "+health);
    }
    public virtual Ability ActivateSkill(int number, ClickLogic cl, Piece p) {
        abilities[number].Activate(cl, p);
        return abilities[number];
    }
    public override void CreateCharGeo(Piece p){
        Debug.Log(charGeo);
        geo = (GameObject)GameObject.Instantiate(Resources.Load(charGeo), p.geo.transform.position + (Vector3.up*0.4f), Quaternion.identity);
        geo.transform.parent = p.geo.transform;
    }
}

public class Warrior : Character{   
    
    public Warrior(){
       name = "Warrior";
       health = 6;
       mana = 10;
       damage = 2;
       abilities = new List<Ability>();
       abilities.Add(new AddPassiveDamageAbility());
       charGeo = "WarriorGeoPrefab";
    }
    /*
    public override void ActivateSkill(Vector3 hitPos){
        this.parentPiece.geo.rigidbody.isKinematic = true;
        parentPiece.GS.activePiece = this.parentPiece;
        parentPiece.GS.MoveEnded();
    }
    */
    private void ShootProjectile(){
    }
}
public class Priest : Character{
    public Priest(){
        name = "Priest";
        health = 6;
        mana = 15;
        damage = 1;
        heal = 2;
        abilities = new List<Ability>();
        abilities.Add(new AddPassiveHealAbility());
        charGeo = "PriestGeoPrefab";
    }
}

public class Archer : Character{
	public Archer()
	{
		name = "Archer";
		health = 5;
		mana = 20;
		damage = 2;
        abilities = new List<Ability>();
        abilities.Add(new ArrowAbility());
        charGeo = "ArcherGeoPrefab";
	}
    /*
    public override void ActivateSkill(Vector3 hitPos){
        Vector2 pos = new Vector2(hitPos.x, hitPos.z);
        parentPiece.GS.RegisterProjectile(new Piece(new Arrow(), pos.x,pos.y, 0.6f, Color.blue), this.parentPiece);
    }
    */
}

public class CharactersLogic : MonoBehaviour 
{

}
