using System.Collections;
using System.Collections.Generic;
using System.Linq;
using chamwhy;
using chamwhy.Components;
using UnityEngine;

public class TA_EnterSector : ITriggerActivate
{
    public int sectorId;
    public int[] injectionIds;
    public Trigger trigger;

    public TA_EnterSector(int sectorId,int[] injectionIds, Trigger trigger)
    {
        this.sectorId = sectorId;
        this.injectionIds = injectionIds;
        this.trigger = trigger;
    }
    public void Activate()
    {
        GameManager.SectorMag.enteredSectors.Add(sectorId);
        GameManager.SectorMag.EnterSectorTrigger(sectorId,this);
        int lastIndex = GameManager.SectorMag.CurrentSectors[SectorManager.SectorType.MainWorld];
        
        if (GameManager.SectorMag.loadedSectors.TryGetValue(lastIndex, out var sector))
        {
            sector.Exit();
        }
        
        if (GameManager.SectorMag.loadedSectors.TryGetValue(sectorId, out sector))
        {
            sector.Enter();
        }
        else
        {
            GameManager.SectorMag.LoadSector(sectorId, () =>
            {
                if (GameManager.SectorMag.loadedSectors.TryGetValue(sectorId, out var value))
                {
                    value.Enter();
                }
            });
        }
        foreach (var injectionIndex in injectionIds)
        {
            GameManager.SectorMag.LoadSector(injectionIndex, null);
        }
        
        // 3. 필요한 섹터 목록 계산 (Manager의 Helper 사용)
        HashSet<int> requiredSectors = GameManager.SectorMag.GetRequiredSectorsBasedOnEnteredTriggers();
        
        List<int> currentlyLoaded = GameManager.SectorMag.loadedSectors.Keys.ToList();
        List<int> unLoadList = currentlyLoaded.Except(requiredSectors).ToList();
        
        foreach (var index in unLoadList)
        {
            // requiredSectors에 없는지 다시 확인 (이론상 불필요하나 안전 장치)
            if (!requiredSectors.Contains(index))
            {
                GameManager.SectorMag.UnloadSector(index, null);
            }
        }
    }
}
