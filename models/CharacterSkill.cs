using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.models
{
    public class CharacterSkill
    {
        public int CharacterId { get; set; }
        public Character Character { get; set; }
        public int SKillId {get;set;}
        public Skill Skill { get; set; }
    }
}