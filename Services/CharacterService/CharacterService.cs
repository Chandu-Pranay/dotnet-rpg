using System.Security.Claims;
using AutoMapper;
using dotnet_rpg.models;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper,DataContext context,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId()=> int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character=_mapper.Map<Character>(newCharacter);
            character.User=await _context.Users.FirstOrDefaultAsync(u=>u.Id==GetUserId());

            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync(); 
            serviceResponse.Data = (_context.Characters.Where(e=>e.User.Id==GetUserId()).Select(c=>_mapper.Map<GetCharacterDto>(c))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
             var serviceResponse=new ServiceResponse<List<GetCharacterDto>>();

            try{
                var character=await _context.Characters.FirstOrDefaultAsync(c=> c.Id==id && c.User.Id==GetUserId());
                if(character is null)
                    throw new Exception($"Character with id '{id}' not found ");

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                
                serviceResponse.Data=_context.Characters.Where(c=>c.User.Id==GetUserId())
                    .Select(c=>_mapper.Map<GetCharacterDto>(c)).ToList();
            }catch(Exception ex){
                serviceResponse.Success=false;
                serviceResponse.Message=ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters=await _context.Characters.Where(c=>c.User.Id==GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var DbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id==GetUserId());
            serviceResponse.Data =_mapper.Map<GetCharacterDto>(DbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse=new ServiceResponse<GetCharacterDto>();

            try{
                var character=await _context.Characters.Include(c=>c.User).FirstOrDefaultAsync(c=> c.Id== updatedCharacter.Id);
                if(character.User.Id==GetUserId())
                   {
                character.Name=updatedCharacter.Name;
                character.HitPoints=updatedCharacter.HitPoints;
                character.Strength=updatedCharacter.Strength;
                character.Defense=updatedCharacter.Defense;
                character.Intelligence=updatedCharacter.Intelligence;
                character.Class=updatedCharacter.Class;
                
                _context.Characters.Update(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data=_mapper.Map<GetCharacterDto>(character);
                   }else{
                     throw new Exception($"Character with id '{updatedCharacter.Id}' not found ");
                   }
            }catch(Exception ex){
                serviceResponse.Success=false;
                serviceResponse.Message=ex.Message;
            }
            return serviceResponse;
        }
    }
}