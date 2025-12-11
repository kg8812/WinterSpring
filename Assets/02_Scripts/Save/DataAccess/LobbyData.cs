
namespace Save.Schema
{
    public class LobbyData
    {
        public LobbySaveData Data => GameManager.Save.GetData(SlotDataKeys.DataTypes.Lobby,GameManager.Save.currentSlotData.slotId) as LobbySaveData;

        public void UnLock(int id)
        {
            Data.ConvenienceOpenedList.Add(id);
            GameManager.instance.WhenUnlock.Invoke();
            Save();
        }

        public bool IsOpen(int id)
        {
            return Data.ConvenienceOpenedList.Contains(id);
        }

        public void Save()
        {
            GameManager.Save.SaveData(SlotDataKeys.GetKey(SlotDataKeys.DataTypes.Lobby,GameManager.Save.currentSlotData.slotId), Data);
        }
    }
}