using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class Teleporter : MonoBehaviour ,Iinteractable
{

    [SerializeField]
    Teleporter TPTo;

    public void Interact(NetworkPlayer whoInteracted)
    {
        if (TPTo == null) return;
        
        whoInteracted.transform.position = new Vector3(TPTo.transform.position.x, 
                                                       TPTo.transform.position.y,
                                                       whoInteracted.transform.position.z);
    }

    private void Awake()
    {
        GetComponent<DebugableObject>().AddGizmoAction(DrawLineToTp);
    }


    void DrawLineToTp()
    {
        if (TPTo == null) return;
        Color color = Color.green;
        Vector3 dir = TPTo.transform.position - transform.position;
        DrawArrow.ForGizmo(transform.position, dir.normalized * 5,color,1,30);
    }

}
public interface Iinteractable
{
    void Interact(NetworkPlayer whoInteracted);
}