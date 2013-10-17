using UnityEngine;
using System.Collections;

public class ClickLogic : MonoBehaviour {

	// Use this for initialization
    private string activeMode = "default";
    private GameObject activePiece = null;
    private GameScript GS = GameScript.Instance;
    private UserVisuals vis = UserVisuals.Instance;
    private float maxPower = 5;
    private Ability activeAbility;

    private GameObject cam;
    private Vector3 prevPos;
	void Start () {
        cam = GameObject.FindWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (activeMode == "default"){
            if (Input.GetMouseButtonDown(0)){
                int pieceLayerMask = 1 << 8;
                RaycastHit hit =  Click(pieceLayerMask);
                if (hit.collider != null){
                    if (GS.IsPieceValid(hit.collider.gameObject.GetComponent<PiecePointerScript>().piece)){
                        if (activePiece == null){
                            activePiece = hit.collider.gameObject;
                            vis.CreateSelection(activePiece.transform.position);
                        }
                        else if(hit.collider.gameObject != activePiece){
                            activePiece = hit.collider.gameObject;
                        }
                        else{
                            activeMode = "pieceDrag";  
                            vis.CreatePowerIndicator(activePiece.transform.position);
                        }
                    }
                }
            }
            if(Input.GetMouseButtonDown(1)){
                int floorLayerMask = 1 << 9;
                RaycastHit hit = Click(floorLayerMask);
                prevPos = hit.point;
            }
            if(Input.GetMouseButton(1)){
                int floorLayerMask = 1 << 9;
                RaycastHit hit = Click(floorLayerMask);
                Vector3 deltaPos = (prevPos - hit.point).normalized;
                prevPos = hit.point;
                cam.SendMessage("RightClickMove", deltaPos);
            }
        }

        if(activeMode == "pieceDrag")
        {
            int floorLayerMask = 1 << 9;
            RaycastHit hit = Click(floorLayerMask);
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider.gameObject != null){
                    vis.DestroyPowerIndicator();
                    CalculateAndMove(activePiece, hit);
                } 
                activeMode = "default";
                activePiece = null;
                vis.DestroyIndicator();
            }
            else{
                float dist = CalculateDist(activePiece, hit.point);
                vis.UpdatePowerIndicator(hit.point, dist);
            }
        }

        if(activeMode == "skillActive"){
            if(Input.GetMouseButtonDown(0)){
                int floorLayerMask = 1 << 9;
                RaycastHit hit = Click(floorLayerMask);
                if(hit.collider.gameObject != null){
                    activeAbility.OnClick(hit.point);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && activePiece != null){
            Character activeChar = (Character)activePiece.GetComponent<PiecePointerScript>().piece.chur;
            activeMode = "skillActive";
            activeAbility = activeChar.ActivateSkill(0, this, activePiece.GetComponent<PiecePointerScript>().piece);
        }
    }
    private void CalculateAndMove(GameObject p, RaycastHit hit){
        hit.point = new Vector3(hit.point.x, 0, hit.point.z);
        Vector3 targetPoint = new Vector3(p.transform.position.x, 1, p.transform.position.z);
        Vector3 angle = (hit.point - (targetPoint)).normalized;
        float dist = CalculateDist(p, hit.point);
        float distPercent = Mathf.Clamp(dist/5, 0, 1);

        p.GetComponent<PiecePointerScript>().piece.Move(-angle, distPercent);
    }

    private float CalculateDist(GameObject p, Vector3 pos){
        float dist = Vector3.Distance(pos, p.transform.position);
        return Mathf.Min(maxPower, dist);
        
    }

    private RaycastHit Click(int layerMask){
        RaycastHit hit;
        Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            return hit;
        else
            return hit;
    }

    public void DeactivatePiece(){
        activePiece = null;
    }
    public void FinishSkill(){
        activeMode = "default";
    }
}
