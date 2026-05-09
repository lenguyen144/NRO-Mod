using NRO_Mod.Models;

namespace NRO_Mod.Features
{
    public class MarkingFeature
    {
        public static MyVector vMarkedMob;

        public static void loadData()
        {
            loadVMarkedMob();
        }

        public static void loadVMarkedMob()
        {
            vMarkedMob.removeAllElements();

            MyVector vMarkedMobByID = SettingsModel.markedModel.vMarkedMobByID;
            for (int i = 0; i < vMarkedMobByID.size(); i++)
            {
                MobModel mobModel = (MobModel)vMarkedMobByID.elementAt(i);
                if (mobModel.mapID != TileMap.mapID) continue;

                Mob mob = GameScr.findMobInMap(mobModel.id);
                if (mob != null)
                {
                    vMarkedMob.addElement(mob);
                }
            }
        }

        public static void clearVMarkedMob()
        {
            vMarkedMob.removeAllElements();
            SettingsModel.markedModel.vMarkedMobByID.removeAllElements();
        }

        /// <summary>
        /// Nếu mob đã đánh dấu thì hủy đánh dấu và ngược lại.| 
        /// hàm này sẽ lấy mapID hiện tại mà người chơi đang ở để thêm thông tin cho mobMode nên dùng hàm này phải cẩn thận.| 
        /// id: là id của mob cần đánh dấu không phải id chung của loại quái đấy.
        /// </summary>
        public static void toggleMarkedMob(int id)
        {
            int index = SettingsModel.markedModel.findMarkedMob(id);
            if (index != -1)
            {
                SettingsModel.markedModel.removeMarkedMobAt(index);
                loadVMarkedMob();
            }
            else
            {
                if (GameScr.findMobInMap(id) != null)
                {
                    int mapID = TileMap.mapID;
                    SettingsModel.markedModel.MarkedMob(id, mapID);

                    loadVMarkedMob();
                }
            }
        }

    }
}
