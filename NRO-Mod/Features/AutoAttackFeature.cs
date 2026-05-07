using UnityEngine;

namespace NRO_Mod.Features
{
    public class AutoAttackFeature
    {
        public static Mob target = null;

        public static void execute()
        {
            if (!Models.SettingsModel.IsAutoAttack ) return;

            if (target == null || target.status == 1 || target.status == 0 || target.levelBoss > 0) target = getRamdomMob();
            if (target == null) return;

            global::Char me = global::Char.myCharz();
            if (me == null) return;

            me.focusManualTo(target);

            global::Skill currentSkill = me.myskill;
            if (currentSkill == null) return;

            long currentTime = global::mSystem.currentTimeMillis();
            if (currentTime - currentSkill.lastTimeUseThisSkill >= currentSkill.coolDown)
            {
                GameScr.gI().doSelectSkill(currentSkill, true);
            }

        }

        /// <summary>
        /// Lấy một con quái bất kì trong map
        /// </summary>
        public static Mob getRamdomMob()
        {
            for (int i = 0; i < global::GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)global::GameScr.vMob.elementAt(i);

                if (mob.status == 0 || mob.status == 1 || mob.levelBoss > 0 || mob.isMobMe) continue;

                return mob;
            }

            return null;
        }
    }
}
