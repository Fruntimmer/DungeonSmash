using UnityEngine;
using System.Collections;

public class Ability{
    protected UserVisuals vis = UserVisuals.Instance;
    protected Piece activePiece;
    protected ClickLogic cl;
    public Ability(){

    }
    public virtual void Activate(ClickLogic cl, Piece p){

    }

    public virtual void OnClick(Vector3 clickPos){

    } 
}

public class ArrowAbility : Ability{
    public override void Activate(ClickLogic cl, Piece p){
        this.cl = cl;
        activePiece = p;
        vis.DestroyIndicator();
        vis.CreateIndicator(p.geo.transform.position, 1.3f, vis.dragColor);
    }
    public override void OnClick(Vector3 clickPos){
        float dist = Vector3.Distance(clickPos, activePiece.geo.transform.position);
        if (dist < 1.3f && dist > 0.8f){
            //activeMode = "default";
            vis.DestroyIndicator();
            activePiece = null;

            Vector2 pos = new Vector2(clickPos.x, clickPos.z);
            GameScript.Instance.RegisterProjectile(new Piece(new Arrow(), pos.x,pos.y, 0.6f, Color.blue), activePiece);
            cl.DeactivatePiece();
            cl.FinishSkill();
        }
    }
}

public class AddPassiveHealAbility : Ability{
	public override void Activate (ClickLogic cl, Piece p){
		p.chur.passiveAbilities.Add(new HealPassive());
		cl.FinishSkill();
	}
}
public class AddPassiveDamageAbility : Ability{
	public override void Activate (ClickLogic cl, Piece p){
		p.chur.passiveAbilities.Add(new DamageBuffPassive());
		cl.FinishSkill();
	}
}

public class PassiveAbility{
	protected int timeCounter;
	public int extraDmg {get; protected set;}
	public PassiveAbility(){
		extraDmg = 0;
	}
	public virtual void FriendlyCollision(Piece collided){}
	public bool NewTurn(){
		timeCounter -=1;
		return timeCounter == 0;
	}
}

public class HealPassive : PassiveAbility{
	public HealPassive(){
		timeCounter = 2;
	}
	public override void FriendlyCollision(Piece collided){		
		((Character)collided.chur).Healed(2);
	}
}

public class DamageBuffPassive : PassiveAbility{
	public DamageBuffPassive(){
		timeCounter = 2;
		extraDmg = 1;
	}
	
}