using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class PlayerCutsceneSkeleton : MonoBehaviour
{

    private Player player;
    [SerializeField] private SkeletonAnimation anim;
    public SkeletonAnimation Skeleton => anim;
    [SerializeField] private Animator animator;
    public Animator Animator => animator;
    [SerializeField] private MeshRenderer mesh;

    public EActorDirection Direction {
        get {
            return transform.localScale.x > 0 ? EActorDirection.Right : EActorDirection.Left;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        Disable();
    }
    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
        var originalSkeleton = player.SkeletonTrans.GetComponent<SkeletonMecanim>();

        anim.skeletonDataAsset = originalSkeleton.skeletonDataAsset;
        anim.Initialize(true);
        anim.skeleton.SetSkin(originalSkeleton.skeleton.Skin);
        anim.skeleton.SetSlotsToSetupPose();
        
        // Material[] mats = originalSkeleton.GetComponent<MeshRenderer>().sharedMaterials;
        
        // mesh.sharedMaterials = mats;
    }
    public void Enable()
    {
        if(mesh.enabled) return;

        mesh.enabled = true;
    }

    public void Disable()
    {
        if(!mesh.enabled) return;

        mesh.enabled = false;
    }

    public void SetDirection(EActorDirection dir)
    {
        Vector3 size = transform.localScale;
        transform.localScale = new Vector3((int)dir * Mathf.Abs(size.x), size.y, size.z);
    }
}
