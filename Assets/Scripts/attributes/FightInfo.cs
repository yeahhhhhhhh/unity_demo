using System;
using System.Collections.Generic;

public class FightInfo
{
    public Int32 atk_ = 0;
    public Int32 def_ = 0;
    public Int32 max_hp_ = 0;
    public Int32 cur_hp_ = 0;
    public Int32 max_mp_ = 0;
    public Int32 cur_mp_ = 0;
    public Int32 speed_ = 0;
    public Int32 critical_rate_ = 0;
    public Int32 critical_damage_ = 0;
    public Int32 evasion_rate_ = 0;
    public Int32 hit_rate_ = 0;

    public void Copy(FightInfo fight_info)
    {
        atk_ = fight_info.atk_;
        def_ = fight_info.def_;
        max_hp_ = fight_info.max_hp_;
        cur_hp_ = fight_info.cur_hp_;
        max_mp_ = fight_info.max_mp_;
        cur_mp_ = fight_info.cur_mp_;
        speed_ = fight_info.speed_;
        critical_rate_ = fight_info.critical_rate_;
        critical_damage_ = fight_info.critical_damage_;
        evasion_rate_ = fight_info.evasion_rate_;
        hit_rate_ = fight_info.hit_rate_;
    }

    public void Copy(attributes.combat.FightInfo fight_info)
    {
        atk_ = fight_info.Atk;
        def_ = fight_info.Def;
        max_hp_ = fight_info.MaxHp;
        cur_hp_ = fight_info.CurHp;
        max_mp_ = fight_info.MaxMp;
        cur_mp_ = fight_info.CurMp;
        speed_ = fight_info.Speed;
        critical_rate_ = fight_info.CriticalRate;
        critical_damage_ = fight_info.CriticalDamage;
        evasion_rate_ = fight_info.EvasionRate;
        hit_rate_ = fight_info.HitRate;
    }
}
