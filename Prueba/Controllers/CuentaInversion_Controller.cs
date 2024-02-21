using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaInversion_Controller : ControllerBase
    {
        private readonly Context _dbContext;

        public CuentaInversion_Controller(Context context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Constructor del controlador `CuentaInversion_Controller`.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>

        [HttpGet]
        [Route("SeleccionarPorCliente/{clienteId:int}")]
        public async Task<IActionResult> SeleccionarPorCliente(int clienteId)
        {
            var cuentas = await _dbContext.CuentaInversions.Where(c => c.IdCliente == clienteId).ToListAsync();
            return Ok(cuentas);
        }

        /// <summary>
        /// Obtiene las cuentas de inversión asociadas a un cliente específico.
        /// </summary>
        /// <param name="clienteId">ID del cliente.</param>
        /// <returns>Lista de cuentas de inversión del cliente.</returns>

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] CuentaInversion request)
        {
            _dbContext.CuentaInversions.Add(request);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, "Cuenta de inversión creada exitosamente");
        }

        /// <summary>
        /// Guarda una nueva cuenta de inversión en la base de datos.
        /// </summary>
        /// <param name="request">Datos de la cuenta de inversión a guardar.</param>
        /// <returns>Respuesta HTTP que indica el resultado de la operación.</returns>

        [HttpPut]
        [Route("Editar/{id:int}")]
        public async Task<IActionResult> Editar(int id, [FromBody] CuentaInversion request)
        {
            if (id != request.Id)
            {
                return BadRequest("El ID de la cuenta de inversión no coincide con el ID especificado en la solicitud");
            }

            try
            {
                _dbContext.Entry(request).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaInversionExists(id))
                {
                    return NotFound("No se encontró la cuenta de inversión especificada");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Edita una cuenta de inversión existente en la base de datos.
        /// </summary>
        /// <param name="id">ID de la cuenta de inversión a editar.</param>
        /// <param name="request">Datos actualizados de la cuenta de inversión.</param>
        /// <returns>Respuesta HTTP que indica el resultado de la operación.</returns>

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cuentaInversion = await _dbContext.CuentaInversions.FindAsync(id);
            if (cuentaInversion == null)
            {
                return NotFound("No se encontró la cuenta de inversión especificada");
            }

            _dbContext.CuentaInversions.Remove(cuentaInversion);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina una cuenta de inversión de la base de datos.
        /// </summary>
        /// <param name="id">ID de la cuenta de inversión a eliminar.</param>
        /// <returns>Respuesta HTTP que indica el resultado de la operación.</returns>

        private bool CuentaInversionExists(int id)
        {
            return _dbContext.CuentaInversions.Any(e => e.Id == id);
        }

        /// <summary>
        /// Verifica si existe una cuenta de inversión con el ID especificado.
        /// </summary>
        /// <param name="id">ID de la cuenta de inversión a verificar.</param>
        /// <returns>Valor booleano que indica si la cuenta de inversión existe.</returns>
    }
}
