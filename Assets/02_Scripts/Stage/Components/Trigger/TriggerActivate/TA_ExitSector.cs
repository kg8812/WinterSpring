using System.Collections.Generic;
using System.Linq;
using chamwhy;
using Sirenix.Utilities;

public class TA_ExitSector : ITriggerActivate
{ 
    private int sectorId;

    public TA_ExitSector(int sectorId)
    {
        this.sectorId = sectorId;
    }
    public void Activate()
    {
        GameManager.SectorMag.enteredSectors.Remove(sectorId);
        GameManager.SectorMag.ExitSectorTrigger(sectorId);

        if (GameManager.SectorMag.loadedSectors.TryGetValue(sectorId, out Sector sector))
        {
            sector.Exit();
        }
        GameManager.SectorMag.enteredSectors.ForEach(x =>
        {
            if (GameManager.SectorMag.loadedSectors.TryGetValue(x, out sector))
            {
                sector.Enter();
            }
        });

        if (GameManager.SectorMag.EnteredSectorTriggers.Count == 0) return;
        
        HashSet<int> requiredSectors = GameManager.SectorMag.GetRequiredSectorsBasedOnEnteredTriggers();

        // 4. 언로드할 섹터 목록 결정: 현재 로드된 것 중 새 필요 목록에 없는 것
        List<int> currentlyLoaded = GameManager.SectorMag.loadedSectors.Keys.ToList();
        List<int> unLoadList = currentlyLoaded.Except(requiredSectors).ToList();

        foreach (var index in unLoadList)
        {
            // requiredSectors에 없는지 다시 확인
            if (!requiredSectors.Contains(index))
            {
                GameManager.SectorMag.UnloadSector(index, null);
            }
        }
    }
}
