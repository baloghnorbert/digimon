using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using Backend.Core.Constans;
using Backend.Core.Enums;
using Backend.Core.Modell.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Backend.Core.Modell.Request;
using System.Threading.Tasks;
using System.Linq;

namespace Backend.API.Controllers
{
    [ApiController]
    public partial class DigimonController : ControllerBase
    {
        private static List<Digimon> _digimons = null;

        static DigimonController()
        {
            _digimons = Digimons().Result;
        }

        public DigimonController()
        {
        }

        [ApiExplorerSettings(GroupName = ApplicationSettingsConstans.PublicAPI)]
        [SwaggerOperation(OperationId = "digimons")]
        [Route("api/v{version:apiVersion}/digimons")]
        [ApiVersion(ApplicationSettingsConstans.ActiveVersion)]
        [HttpGet]
        [ProducesResponseType((int)HttpResponseType.OK, Type = typeof(List<Digimon>))]
        [ProducesResponseType((int)HttpResponseType.BadRequest)]
        [Produces("application/json")]
        public List<Digimon> GetAll()
        {
            try
            {
                return _digimons;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [ApiExplorerSettings(GroupName = ApplicationSettingsConstans.PublicAPI)]
        [SwaggerOperation(OperationId = "paged")]
        [Route("api/v{version:apiVersion}/digimons/page/{page}")]
        [ApiVersion(ApplicationSettingsConstans.ActiveVersion)]
        [HttpGet]
        [ProducesResponseType((int)HttpResponseType.OK, Type = typeof(List<Digimon>))]
        [ProducesResponseType((int)HttpResponseType.BadRequest)]
        [Produces("application/json")]
        public List<Digimon> Page([FromRoute] [Required] int page = 0)
        {
            try
            {
                return _digimons.Skip(page * 10)
                                .Take(10)
                                .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [ApiExplorerSettings(GroupName = ApplicationSettingsConstans.PublicAPI)]
        [SwaggerOperation(OperationId = "byId")]
        [Route("api/v{version:apiVersion}/digimon/{id}")]
        [ApiVersion(ApplicationSettingsConstans.ActiveVersion)]
        [HttpGet]
        [ProducesResponseType((int)HttpResponseType.OK, Type = typeof(Digimon))]
        [ProducesResponseType((int)HttpResponseType.BadRequest)]
        [Produces("application/json")]
        public Digimon GetById([FromRoute] [Required] int id)
        {
            try
            {
                return GetDigimonById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [ApiExplorerSettings(GroupName = ApplicationSettingsConstans.PublicAPI)]
        [SwaggerOperation(OperationId = "delete")]
        [Route("api/v{version:apiVersion}/digimon/delete/{id}")]
        [ApiVersion(ApplicationSettingsConstans.ActiveVersion)]
        [HttpDelete]
        [ProducesResponseType((int)HttpResponseType.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpResponseType.BadRequest)]
        [Produces("application/json")]
        public bool Delete([FromRoute][Required] int id)
        {
            try
            {
                DeletDigimon(id);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [ApiExplorerSettings(GroupName = ApplicationSettingsConstans.PublicAPI)]
        [SwaggerOperation(OperationId = "crate")]
        [Route("api/v{version:apiVersion}/digimon/create")]
        [ApiVersion(ApplicationSettingsConstans.ActiveVersion)]
        [HttpPost]
        [ProducesResponseType((int)HttpResponseType.OK, Type = typeof(Digimon))]
        [ProducesResponseType((int)HttpResponseType.BadRequest)]
        [Produces("application/json")]
        public Digimon CreateAsync([FromBody] [Required] DigimonRequest requestParam)
        {
            try
            {
                Digimon digimon = AddDigimon(requestParam);
                return digimon;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [ApiExplorerSettings(GroupName = ApplicationSettingsConstans.PublicAPI)]
        [SwaggerOperation(OperationId = "update")]
        [Route("api/v{version:apiVersion}/digimon/update")]
        [ApiVersion(ApplicationSettingsConstans.ActiveVersion)]
        [HttpPut]
        [ProducesResponseType((int)HttpResponseType.OK, Type = typeof(Digimon))]
        [ProducesResponseType((int)HttpResponseType.BadRequest)]
        [Produces("application/json")]
        public Digimon UpdateAsync([FromBody][Required] Digimon requestParam)
        {
            try
            {
                UpdateDigimon(requestParam);
                return requestParam;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
