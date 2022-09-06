using AutoMapper;
using Back_end.DTOs;
using Back_end.Entidades;
using Back_end.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileSaver fileSaver;
        private readonly string container = "actores";

        public ActoresController(ApplicationDbContext context, IMapper mapper, IFileSaver fileSaver)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileSaver = fileSaver;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Actores.AsQueryable();
            await HttpContext.InsertPaginationParamsInHeader(queryable);
            var actores = await queryable.OrderBy(x => x.Nombre).Page(paginacionDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(actores);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int Id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == Id);
            if (actor == null)
            {
                return NotFound();
            }
            return mapper.Map<ActorDTO>(actor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CrearActorDTO crearActorDTO)
        {
            var actor = mapper.Map<Actor>(crearActorDTO);
            if (crearActorDTO.Foto != null)
            {
                actor.Foto = await fileSaver.SaveFile(container, crearActorDTO.Foto);
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id, [FromForm] CrearActorDTO crearActorDTO)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == Id);
            if (actor == null)
            {
                return NotFound();
            }
            actor = mapper.Map(crearActorDTO, actor);

            if (crearActorDTO.Foto != null)
            {
                actor.Foto = await fileSaver.UpdateFile(container, crearActorDTO.Foto, actor.Foto);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NoContent();
            }
            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();

            await fileSaver.DeleteFile(actor.Foto, container);
            return NoContent();
        }
    }
}
