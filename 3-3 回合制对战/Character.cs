using System;
using System.Collections.Generic;
using System.Text;

namespace _3_3_回合制对战
{
    // 技能类型
    public enum SkillType
    {
        NormalAttack,    // 普攻
        SuckBlood,       // 吸血
        Heal,            // 治疗
        HealOverTime,    // 按时间恢复血量
        DamageOverTime,  // 按时间伤害，dot
    }

    class Utils
    {
        public static Random random = new Random();
    }


    // 角色类
    class Character
    {
        public string name { get; private set; }
        public int hp { get; private set; }
        public int maxHp { get; private set; }
        public int attack { get; private set; }
        public int def { get; private set; }
        public int critChance;           // 万分比，比如1000代表 10%
        public int critRate = 20000;             // 暴击时伤害的倍数，默认是2倍

        // 伤害 = 对方攻击 * （ 1 - (float)def / (300+def))

        public List<Skill> skills { get; private set; }
 
        public Character(string name, int hp, int attack, int def, int critChance)
        {
            this.name = name;
            this.maxHp = hp;
            this.hp = maxHp;
            this.attack = attack;
            this.def = def;
            this.critChance = critChance;

            skills = new List<Skill>();
        }

        public void AddSkill(Skill skill)
        {
            skills.Add(skill);
        }

        public void CostHp(int cost)
        {
            this.hp -= cost;
            if (this.hp <= 0)
            { this.hp = 0; }
        }

        public void AddHp(int add)
        {
            this.hp += add;
            if(hp > maxHp) { hp = maxHp; }
            // hp = Math.Min(hp,maxHp);
        }
        public void Attack(Skill skill, Character other)
        {
            switch (skill.type)
            {
                case SkillType.NormalAttack:
                    {
                        float a = attack * (skill.data1 / 10000f);
                        int damage = (int)(a * (1 - (float)other.def / (300 - other.def)));

                        other.CostHp(damage);

                        Console.WriteLine($"{this.name}使用{skill.name}对{other.name}造成{damage}点伤害。");
                        Console.WriteLine($"{other.name} 体力：{other.hp}/{other.maxHp}");
                    }
                    break;
                case SkillType.SuckBlood:
                    {
                        float a = attack * (skill.data1 / 10000f);
                        int damage = (int)(a * (1 - (float)other.def / (300 - other.def)));

                        int suck = (int)(damage * (skill.data2 / 10000f));
                        other.CostHp(damage);
                        this.AddHp(suck);
                        Console.WriteLine($"{this.name}使用{skill.name}对{other.name}造成{damage}点伤害~");
                        Console.WriteLine($"{other.name} 体力：{other.hp}/{other.maxHp}");
                        Console.WriteLine($"{name} 吸取了{suck}点体力。体力：{hp}/{maxHp}");
                    }
                    break;
                case SkillType.Heal:
                    {
                        int heal = (int)(attack * (skill.data1 / 10000f));
                        other.AddHp(heal);

                        Console.WriteLine($"{name} 使用了治疗技能，治疗{heal}点体力~~");
                        Console.WriteLine($"{name} 体力：{hp}/{maxHp}");
                    }
                    break;
            }
        }

        
        public bool IsDead()
        {
            return this.hp <= 0;
        }


    }

    class Skill
    {
        public string name;
        public SkillType type;

        public bool self = false;

        // 普攻：攻击比例，万分比。
        // 治疗：治疗比例，万分比。
        public int data1;
        // 吸血：吸取对方生命力万分比
        public int data2;   

        public Skill(string name, SkillType type, int data1,int data2 = 0)
        {
            this.name = name;
            this.type = type;
            this.data1 = data1;
            this.data2 = data2;
        }

        public Skill(string name, SkillType type, bool self,int data1,int data2=0)
        {
            this.name = name;
            this.type = type;
            this.self = self;
            this.data1 = data1;
            this.data2 = data2;
            
        }
    }
}
