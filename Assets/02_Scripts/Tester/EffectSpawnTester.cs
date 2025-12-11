using Sirenix.OdinInspector;
using UnityEngine;

public class EffectSpawnTester : MonoBehaviour
{
    public string EffectName;

    private Player player => GameManager.instance.Player;

    private GameObject effect;
    
    [Button]
    public void Spawn(float scale)
    {
        effect = player.EffectSpawner.Spawn(EffectName, GameManager.instance.ControllingEntity.Position, false).gameObject;
        effect.transform.localScale = Vector3.one * scale;
    }

    [Button]
    public void Remove()
    {
        player.EffectSpawner.Remove(EffectName);
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.U))
        {
            effect.transform.Translate(Vector3.left * (Time.deltaTime * 5));
        }
        else if (Input.GetKey(KeyCode.I))
        {
            effect.transform.Translate(Vector3.right * (Time.deltaTime * 5));
        }
    }
}
