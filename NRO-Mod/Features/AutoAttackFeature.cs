using UnityEngine;

namespace NRO_Mod.Features
{
    public class AutoAttackFeature
    {
        protected static Mob target = null;

        protected static long timeDelay = 1000;
        protected static long timeLast = 0;

        public static void execute()
        {
            global::Char me = global::Char.myCharz();
            if (me == null) return;

            if (me.isDie && me.statusMe == 14) return;

            if (target == null || target.status == 1 || target.status == 0 || target.levelBoss > 0) target = getRamdomMob();
            if (target == null) return;

            me.focusManualTo(target);

            global::Skill currentSkill = me.myskill;
            if (currentSkill == null) return;

            long currentTime = global::mSystem.currentTimeMillis();
            if (Res.abs(me.cx - target.x) > currentSkill.dx || Res.abs(me.cy - target.y) > currentSkill.dy)
            {
                if (currentTime - timeLast >= timeDelay)
                {
                    GameScr.gI().doSelectSkill(currentSkill, true);
                    timeLast = currentTime;
                }
            }
            else if (currentTime - currentSkill.lastTimeUseThisSkill >= currentSkill.coolDown)
            {
                GameScr.gI().doSelectSkill(currentSkill, true);
                timeLast = 0;
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

        public static Mob getMarkedMob()
        {
            for (int i = 0; i < MarkingFeature.vMarkedMob.size(); i++)
            {
                Mob mob = (Mob)MarkingFeature.vMarkedMob.elementAt(i);

                if (mob.status == 0 || mob.status == 1 || mob.levelBoss > 0 || mob.isMobMe) continue;

                return mob;
            }

            return null;
        }

        public static void reset()
        {
            target = null;
        }
    }
}
