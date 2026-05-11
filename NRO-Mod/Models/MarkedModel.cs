
namespace NRO_Mod.Models
{
    public class MarkedModel
    {
        public MyVector vMarkedMobByID = new MyVector();
        public int area;

        public int findMarkedMob(int id, int mapID)
        {
            for (int i = 0; i < vMarkedMobByID.size(); i++)
            {
                MobModel mobModel = (MobModel)vMarkedMobByID.elementAt(i);
                if (mobModel.id == id && mobModel.mapID == mapID) return i;
            }

            return -1;
        }

        public void removeMarkedMobAt(int index)
        {
            vMarkedMobByID.removeElementAt(index);
        }

        public void MarkedMob(int id, int mapID)
        {
            MobModel mobModel = new MobModel();
            mobModel.id = id;
            mobModel.mapID = mapID;

            vMarkedMobByID.addElement(mobModel);
        }

    }
}
