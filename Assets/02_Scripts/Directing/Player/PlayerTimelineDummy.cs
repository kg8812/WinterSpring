using Apis;
using Spine.Unity;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public partial class PlayerTimelineDummy : MonoBehaviour
{
    // Start is called before the first frame update
    public int tid = 9999;
    MeshRenderer _mesh;
    public MeshRenderer Mesh => _mesh ??= GetComponent<MeshRenderer>();

    public Vector3 Position => Mesh.transform.position;

    public EActorDirection Direction {
        get {
            return transform.localScale.x > 0 ? EActorDirection.Right : EActorDirection.Left;
        }
    }

    public EActorDirection WorldDirection {
        get {
            return transform.parent.transform.localScale.x > 0 ? EActorDirection.Right : EActorDirection.Left;
        }
    }

    void Start()
    {
        Disable();
    }

    public void Enable()
    {
        if(Mesh.enabled) return;
        Mesh.enabled = true;
    }

    public void Disable()
    {
        if(!Mesh.enabled) return;
        Mesh.enabled = false;
    }
}
