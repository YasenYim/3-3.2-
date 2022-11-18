﻿using System;
using System.Collections.Generic;

namespace _3_3_回合制对战
{
   
    class Program
    {
        // 输入数字，如果错误重新输入
        static int InputNum()
        {
            while (true)
            {
                Console.WriteLine("请输入一个数字：");
                string input = Console.ReadLine();

                int num;
                bool success = int.TryParse(input, out num); // out输出参数

                if (!success)
                {
                    Console.WriteLine("请重新输入");
                    continue;    
                }

                return num;
            }
        }
        static Skill ChooseSkill(Character cha)
        {
            // 打印角色的技能列表
            for(int i = 0;i<cha.skills.Count;i++)
            {
                Skill skill = cha.skills[i];
                Console.WriteLine($"{i + 1}.{skill.name}");
            }

            // 让用户输入技能序号
            int index = -1;
            while (index < 0 || index >= cha.skills.Count)
            {
                index = InputNum() - 1;
            }

            // 选中技能并返回技能
            Skill choose = cha.skills[index];
            return choose;
        }

        static Skill RandomSkill(Character cha)
        {
            int index = Utils.random.Next(0, cha.skills.Count);
            return cha.skills[index];
        }
        static void Main(string[] args)
        {
            Character player = new Character("三尾狐",875,124,68,1000);
            Skill skill1 = new Skill("尾袭", SkillType.NormalAttack,10000);
            Skill skill2 = new Skill("诱惑", SkillType.SuckBlood, 8000, 2000);
            player.AddSkill(skill1);
            player.AddSkill(skill2);

            Character enemy = new Character("雨女", 1035, 97, 73, 500);
            Skill eskill1 = new Skill("泪珠", SkillType.NormalAttack, 10000);
            Skill eskill2 = new Skill("治疗", SkillType.Heal, true, 7000);
            enemy.AddSkill(eskill1);
            enemy.AddSkill(eskill2);

            int round = 1;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"------第{round}回合------");

                // 玩家攻击敌人
                Console.ForegroundColor = ConsoleColor.Green;
                // 选择技能
                Skill playerSkill = ChooseSkill(player);
                if(playerSkill.self)
                {
                    player.Attack(playerSkill, player);
                }
                else
                {
                    player.Attack(playerSkill, enemy);
                }

                

                // 判断敌人是否死亡
                if(enemy.IsDead())
                { Console.WriteLine($"{enemy.name}战败了！"); break; }

                // 敌人攻击玩家
                Console.ForegroundColor = ConsoleColor.Red;
                // 选择技能
                Skill enemySkill = RandomSkill(enemy);
                if(enemySkill.self)
                {
                    enemy.Attack(enemySkill, enemy);
                }
                else
                {
                    enemy.Attack(enemySkill, player);
                }
              

                // 判断敌人是否死亡
                if (player.IsDead())
                { Console.WriteLine($"{player.name}战败了！"); break; }
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            if (player.IsDead())
            { Console.WriteLine($"{player.name}战败了，游戏结束。"); }
            else
            { Console.WriteLine($"恭喜{player.name}获得胜利！"); }

            Console.ReadKey();
        }
    }
}
