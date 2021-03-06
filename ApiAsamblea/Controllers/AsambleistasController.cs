using ApiAsamblea.Models;
using ApiAsamblea.Models.Dtos;
using ApiAsamblea.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAsamblea.Controllers
{
    [Authorize]
    [Route("api/Asambleistas")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiAsamblea")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class AsambleistasController : Controller
    {
        private readonly IAsambleistaRepository _abRepo;
        private readonly IMapper _mapper;

        public AsambleistasController(IAsambleistaRepository abRepo, IMapper mapper)
        {
            _abRepo = abRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todos los asambleistas
        /// </summary>
        /// <returns></returns>
      
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<AsambleistaDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetAsamleistas()
        {
            var listaAsambleistas = _abRepo.GetAsambleistas();

            var listaAsambleistasDto = new List<AsambleistaDto>();

            foreach (var lista in listaAsambleistas)
            {
                listaAsambleistasDto.Add(_mapper.Map<AsambleistaDto>(lista));
            }

            return Ok(listaAsambleistasDto);

        }

        /// <summary>
        /// Obtener un asambleista individual
        /// </summary>
        /// <param name="asambleistaId">Este es el Id del asambleista</param>
        /// <returns></returns>
        
        [HttpGet("{asambleistaId:int}", Name = "GetAsambleista")]
        [ProducesResponseType(200, Type = typeof(List<AsambleistaDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetAsambleista(int asambleistaId)
        {
            var itemAsambleista = _abRepo.GetAsambleista(asambleistaId);

            if (itemAsambleista == null)
            {
                return NotFound();
            }

            var itemAsambleistaDto = _mapper.Map<AsambleistaDto>(itemAsambleista);
            return Ok(itemAsambleistaDto);
        }

        /// <summary>
        /// Crear un nuevo asambleista
        /// </summary>
        /// <param name="asambleistaDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<AsambleistaDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearAsambleista([FromBody] AsambleistaDto asambleistaDto)
        {
            if (asambleistaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_abRepo.ExisteAsambleista(asambleistaDto.Nombre))
            {
                ModelState.AddModelError("", "El Asambleista ya existe");
                return StatusCode(404, ModelState);
            }

            var asambleista = _mapper.Map<Asambleista>(asambleistaDto);

            if (!_abRepo.CrearAsambleista(asambleista))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{asambleista.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetAsambleista", new { asambleistaId = asambleista.Id }, asambleista);
        }

        /// <summary>
        /// Actualizar un asambleista existente
        /// </summary>
        /// <param name="asambleistaId"></param>
        /// <param name="asambleistaDto"></param>
        /// <returns></returns>
       
        [HttpPatch("{asambleistaId:int}", Name = "ActualizarAsambleista")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarAsambleista(int asambleistaId, [FromBody] AsambleistaDto asambleistaDto)
        {
            if (asambleistaDto == null || asambleistaId != asambleistaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var asambleista = _mapper.Map<Asambleista>(asambleistaDto);

            if (!_abRepo.ActualizarAsambleista(asambleista))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{asambleista.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Eliminar un asambleista existente
        /// </summary>
        /// <param name="asambleistaId"></param>
        /// <returns></returns>
       
        [HttpDelete("{asambleistaId:int}", Name = "BorrarAsambleista")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarAsambleista(int asambleistaId)
        {
            if (!_abRepo.ExisteAsambleista(asambleistaId))
            {
                return NotFound();
            }

            var asambleista = _abRepo.GetAsambleista(asambleistaId);

            if (!_abRepo.BorrarAsambleista(asambleista))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro{asambleista.Nombre}");
                return StatusCode(500, ModelState);
            }


            return NoContent();
        }

    }
}
