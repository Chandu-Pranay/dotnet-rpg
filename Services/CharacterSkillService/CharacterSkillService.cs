using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.CharacterSkill;

namespace dotnet_rpg.Services.CharacterSkillService
{
    public class CharacterSkillService : ICharacterSkillService
    {
        public IMapper _mapper { get; }
        public IHttpContextAccessor _httpContextAccessor {get;}
        public DataContext _context{get;} 
        public CharacterSkillService(DataContext context,IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor=httpContextAccessor;
            _context=context;
        }


        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            ServiceResponse<GetCharacterDto> response=new ServiceResponse<GetCharacterDto>();
            try
            {
                    Character? character=await _context.Characters
                        .Include(c=>c.Weapon)
                        .Include(c=>c.CharacterSkills).ThenInclude(cs=>cs.Skill)
                        .FirstOrDefaultAsync(c=>c.Id==newCharacterSkill.CharacterId &&
                        c.User.Id==int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                   
                    if(character is null)
                    {
                        response.Success=false;
                        response.Message="Character does not exixts";
                        return response;
                    }
                   
                    Skill? skill=await _context.Skills.FirstOrDefaultAsync(s=>s.Id==newCharacterSkill.SkillId);
                   
                    if(skill==null)
                    {
                        response.Success=false;
                        response.Message="SKill not found";
                        return response;
                    }
                   
                    CharacterSkill characterSkill=new CharacterSkill{
                        Character=character,
                        Skill=skill
                    
                    };
                    await _context.CharacterSkills.AddAsync(characterSkill);
                    await _context.SaveChangesAsync();

                    response.Data=_mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception e)
            {
                    response.Success=false;
                    response.Message=e.Message;
            }
            return response;
        }
    }
}