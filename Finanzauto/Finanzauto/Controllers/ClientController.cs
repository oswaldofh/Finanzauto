using AutoMapper;
using Finanzauto.Common.Response;
using Finanzauto.Domain.DTOs;
using Finanzauto.Domain.Entities;
using Finanzauto.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.Net;

namespace Finanzauto.Controllers
{
    [Route("clients")]
    [ApiController]
    
    public class ClientController : ControllerBase
    {
        protected readonly IClientRepository _repository;
        private readonly IVehicleRepository _vehicleRepository;
        protected readonly IMapper _mapper;
        protected ResponseApi _response;

        public ClientController(IClientRepository repository, IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _repository = repository;
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
            _response = new();
        }

        /// <summary>
        /// Obtiene valores de todos los tipos de clientes
        /// </summary>
        /// <response code="200"> Si se obtiene el listado</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repository.GetAll();
            

            return Ok(data);
        }


        /// <summary>
        /// Añade un registro
        /// </summary>
        /// <param name="model">CreateClientDto</param>
        /// <returns>Retorna el registro creado</returns>
        /// <response code="201">Se ha creado correctamente un nuevo registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDto model)
        {

            if (!ModelState.IsValid || model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }


            try
            {
               var data = _mapper.Map<Client>(model);

                await _repository.Save(data);

                Vehicle vehicle = await _vehicleRepository.Get(data.VehicleId);
                vehicle.PhaseId = 4;
                await _vehicleRepository.Update(vehicle);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Messages.Add("se guardo el registro correctamente");
                _response.Result = data;

                return Ok(_response);

            }
            catch (DbUpdateException e)
                when (e.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add($"Ya existe un registro con esos parametros");
                return BadRequest(_response);
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Messages.Add(e.Message);
                return BadRequest(_response);
            }

        }
    }
}
