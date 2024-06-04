using AutoMapper;
using Finanzauto.Common.Enums;
using Finanzauto.Common.Response;
using Finanzauto.Domain.DTOs;
using Finanzauto.Domain.Entities;
using Finanzauto.Domain.Repositories;
using Finanzauto.FileRoot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.Net;

namespace Finanzauto.Controllers
{
    [Route("vehicles")]
    [ApiController]
    public class VehicleController : ControllerBase
    {

        protected readonly IVehicleRepository _repository;
        protected readonly IMapper _mapper;
        private readonly IUploadFileRepository _uploadFileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhaseRepository _phaseRepository;
        protected ResponseApi _response;

        public VehicleController(
            IVehicleRepository repository,
            IMapper mapper,
            IUploadFileRepository uploadFileRepository,
            IUserRepository userRepository,
            IPhaseRepository phaseRepository
        )
        {
            _repository = repository;
            _mapper = mapper;
            _uploadFileRepository = uploadFileRepository;
            _userRepository = userRepository;
            _phaseRepository = phaseRepository;
            _response = new();
        }


        /// <summary>
        /// Obtiene valores de todos los tipos de clientes
        /// </summary>
        /// <response code="200"> Si se obtiene el listado</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            User? user = await _userRepository.GetUserAsync(User.Identity.Name); //SE OBTIENE EL USUARIO LOGUEADO
            
            if (user.UserType.ToString() == "Admin")
            {  
                var data = await _repository.GetAll();
                if (!data.IsNullOrEmpty())
                {
                    var vehicles = new List<InformationVehicleDto>();
                    foreach (var list in data)
                    {

                        var images = new List<PhotoDto>();
                        foreach (var item in list.VehiclePhotos)
                        {
                            item.Image = _uploadFileRepository.GetUrlBase(item.Image);
                            images.Add(_mapper.Map<PhotoDto>(item));
                        }

                        var vehicle = _mapper.Map<InformationVehicleDto>(list);
                        vehicle.Images = images;
                        vehicles.Add(vehicle);
                    }
                    return Ok(vehicles);
                }

            }
            else
            {
                var data = await _repository.GetAllActive();
                if (!data.IsNullOrEmpty())
                {
                    var vehicles = new List<InformationVehicleDto>();
                    foreach (var list in data)
                    {

                        var images = new List<PhotoDto>();
                        foreach (var item in list.VehiclePhotos)
                        {
                            item.Image = _uploadFileRepository.GetUrlBase(item.Image);
                            images.Add(_mapper.Map<PhotoDto>(item));
                        }

                        var vehicle = _mapper.Map<InformationVehicleDto>(list);
                        vehicle.Images = images;
                        vehicles.Add(vehicle);
                    }
                    return Ok(vehicles);
                }

            }


            return Ok();
        }

        /// <summary>
        /// Obtiene el valor pasando el id por parametro
        /// </summary>
        /// <param name="id">Id</param>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repository.Get(id);

            if (data == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese id");
                return NotFound(_response);
            }

            var vehicle = _mapper.Map<InformationVehicleDto>(data);
            var images = new List<PhotoDto>();
            foreach (var item in data.VehiclePhotos)
            {
                item.Image = _uploadFileRepository.GetUrlBase(item.Image);
                images.Add(_mapper.Map<PhotoDto>(item));
            }
            vehicle.Images = images;

            return Ok(vehicle);
        }

        /// <summary>
        /// Obtiene un registro pasando el nombre por parametro
        /// </summary>
        /// <param name="plate">Nombre</param>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [AllowAnonymous]
        [HttpGet("{plate}", Name = "GetByPlate")]
        public async Task<IActionResult> GetByPlate(string plate)
        {
            var data = await _repository.GetByName(plate);
            if (data == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese nombre");
                //return BadRequest(_response);
                return NotFound(_response);
            }
            var vehicle = _mapper.Map<InformationVehicleDto>(data);
            var images = new List<PhotoDto>();
            foreach (var item in data.VehiclePhotos)
            {
                item.Image = _uploadFileRepository.GetUrlBase(item.Image);
                images.Add(_mapper.Map<PhotoDto>(item));
            }
            vehicle.Images = images;

            return Ok(vehicle);
        }



        /// <summary>
        /// Añade un registro
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Retorna el registro creado</returns>
        /// <response code="201">Se ha creado correctamente un nuevo registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateVehicleDto model) //FromBody  FromForm
        {

            if (!ModelState.IsValid || model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }

            var phase = await _phaseRepository.Get(model.PhaseId);
            if (phase == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("no se encuentra la fase");
                return NotFound(_response);
            }


            try
            {
                User? user = await _userRepository.GetUserAsync(User.Identity.Name); //SE OBTIENE EL USUARIO LOGUEADO


                var vehiculoDto = new DataVehicleDto
                {
                    Plate = model.Plate,
                    Color = model.Color,
                    BrandId = model.BrandId,
                    PhaseId = model.PhaseId,
                    Line = model.Line,
                    Year = model.Year,
                    Price = model.Price,
                    Mileage = model.Mileage,
                    Observation = model.Observation,
                };
                var data = _mapper.Map<Vehicle>(vehiculoDto);

                await _repository.Save(data);

                var auditDto = new CreateAuditDto
                {
                    ActionAudit = ActionAudit.Creado,
                    Created = DateTime.Now,
                    VehicleId = data.Id,
                    User = user,
                    PreviousValue = phase.Name,
                    NewValue = phase.Name

                };

                var audit = _mapper.Map<VehicleAudit>(auditDto);
                await _repository.SaveAudit(audit);

                if (model.Images != null && model.Images.Count > 0)
                {
                    foreach (var image in model.Images)
                    {
                        if (image.Length > 0)
                        {
                            var photoDto = new CreatePhotoDto
                            {
                                Image = await _uploadFileRepository.UploadFilesAsync(image, "vehicle"),
                                VehicleId = data.Id
                            };
                            var photo = _mapper.Map<VehiclePhoto>(photoDto);
                            await _repository.SavePhoto(photo);
                        }
                    }
                }

                return Ok();

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


        /// <summary>
        /// Actualiza un registro
        /// </summary>
        /// <param name="model">VehiculoDto</param>
        /// <returns>Retorna el registro acutlizado</returns>
        /// <response code="200">Se ha actualizado correctamente el registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] VehicleDto model)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }

            var phase = await _phaseRepository.Get(model.PhaseId);
            if (phase == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("no se encuentra la fase");
                return NotFound(_response);
            }

            var exist = await _repository.Exist(model.Id);
            if (!exist)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un registro con el id {model.Id}");
                return NotFound(_response);
            }

            User? user = await _userRepository.GetUserAsync(User.Identity.Name); //SE OBTIENE EL USUARIO LOGUEADO

            try
            {
                var vehiculoDto = new EditVehicleDto
                {
                    Id = model.Id,
                    Plate = model.Plate,
                    Color = model.Color,
                    BrandId = model.BrandId,
                    PhaseId = model.PhaseId,
                    Line = model.Line,
                    Year = model.Year,
                    Price = model.Price,
                    Mileage = model.Mileage,
                    Observation = model.Observation,
                };
                var data = _mapper.Map<Vehicle>(vehiculoDto);
                await _repository.Update(data);

                var auditDto = new CreateAuditDto
                {
                    ActionAudit = ActionAudit.Actualizado,
                    Created = DateTime.Now,
                    VehicleId = data.Id,
                    User = user,
                    PreviousValue = phase.Name,
                    NewValue = phase.Name

                };

                var audit = _mapper.Map<VehicleAudit>(auditDto);
                await _repository.SaveAudit(audit);


                if (model.Images != null && model.Images.Count > 0)
                {
                    foreach (var image in model.Images)
                    {
                        if (image.Length > 0)
                        {
                            var photoDto = new CreatePhotoDto
                            {
                                Image = await _uploadFileRepository.UploadFilesAsync(image, "vehicle"),
                                VehicleId = data.Id
                            };
                            var photo = _mapper.Map<VehiclePhoto>(photoDto);
                            await _repository.SavePhoto(photo);
                        }
                    }
                }

                return Ok();

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

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/state")]
        public async Task<IActionResult> UpdateVehicleState(int id, [FromBody] VehiclePhaseDto model)
        {
            var vehicle = await _repository.Get(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var phase = await _phaseRepository.Get(model.PhaseId);
            if (phase != null)
            {
                vehicle.PhaseId = phase.Id;
                await _repository.Update(vehicle);
                return Ok();
            }

            return NotFound();
        }

        /// <summary>
        /// Borra un registro pasando el id por parametro 
        /// </summary>
        /// <param name="id">Id</param>
        /// <response code="204">Si se elimina el registro</response>
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="404">Si no existe el registro</response>
        /// <response code="500">Se ha producido un error interno en el servido</response> 
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}", Name = "Delete")]
        public async Task<IActionResult> Delete(int id)
        {

            var data = await _repository.Exist(id);
            if (!data)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un registro con el id {id}");
                return NotFound(_response);
            }


            var deleted = await _repository.Delete(id);
            if (!deleted)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Messages.Add($"Algo salio mal eliminando el registro {id}");
                return BadRequest(_response);
            }
            User? user = await _userRepository.GetUserAsync(User.Identity.Name); //SE OBTIENE EL USUARIO LOGUEADO

            var auditDto = new CreateAuditDto
            {
                ActionAudit = ActionAudit.Eliminar,
                Created = DateTime.Now,
                VehicleId = id,
                User = user,
                PreviousValue = "Eliminado",
                NewValue = "Eliminado"

            };

            var audit = _mapper.Map<VehicleAudit>(auditDto);
            await _repository.SaveAudit(audit);

            var photosVehicle = await _repository.GetPhotosVehicle(id);
            foreach (var item in photosVehicle)
            {
                var deletedPhoto = await _repository.DeletePhoto(item);
                if (deletedPhoto)
                {
                    await _uploadFileRepository.DeleteFile(item.Image, "vehicle");
                }
            }


            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            _response.Messages.Add("Se elimino el registro correctamente");
            return Ok(_response);
        }
        /// <summary>
        /// Borra un registro pasando el id por parametro 
        /// </summary>
        /// <param name="id">Id</param>
        /// <response code="204">Si se elimina el registro</response>
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="404">Si no existe el registro</response>
        /// <response code="500">Se ha producido un error interno en el servido</response> 
        [Authorize(Roles = "Admin")]

        [HttpDelete("DeletePhoto/{id:int}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {

            var photo = await _repository.GetPhoto(id);
            if (photo == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un registro con el id {id}");
                return NotFound(_response);
            }
            var deleted = await _repository.DeletePhoto(photo);
            if (!deleted)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Messages.Add($"Algo salio mal eliminando el registro {id}");
                return BadRequest(_response);
            }

            await _uploadFileRepository.DeleteFile(photo.Image, "vehicle");

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            _response.Messages.Add("Se elimino el registro correctamente");
            return Ok(_response);
        }
    }
}
