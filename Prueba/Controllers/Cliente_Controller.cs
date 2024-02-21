using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prueba.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cliente_Controller : ControllerBase
    {
        private readonly Context _dbContext;

        public Cliente_Controller(Context context)
        {
            _dbContext = context;
        }

        [HttpGet]
        [Route("Seleccionar")]
        public async Task<IActionResult> Seleccionar()
        {
            List<Models.Cliente> seleccionar = await _dbContext.Clientes.OrderByDescending(e => e.Id).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, seleccionar);
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] Cliente request)
        {
            await _dbContext.Clientes.AddAsync(request);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, "ok");
        }

        [HttpPut]
        [Route("Editar/{id:int}")] // Agregar el parámetro de ruta para el ID del cliente
        public async Task<IActionResult> Editar(int id, [FromBody] Cliente request)
        {
            if (id != request.Id) // Verificar si el ID de la solicitud coincide con el ID de la ruta
            {
                return BadRequest(); // Devolver un error 400 si los IDs no coinciden
            }

            var cliente = await _dbContext.Clientes.FindAsync(id); // Buscar el cliente en la base de datos

            if (cliente == null)
            {
                return NotFound(); // Devolver un error 404 si el cliente no se encuentra
            }

            // Actualizar los campos del cliente con los datos de la solicitud
            cliente.Rut = request.Rut;
            cliente.Nombre = request.Nombre;
            cliente.FechaNacimiento = request.FechaNacimiento;

            try
            {
                await _dbContext.SaveChangesAsync(); // Guardar los cambios en la base de datos
                return StatusCode(StatusCodes.Status200OK, "ok"); // Devolver un éxito 200 OK
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el cliente"); // Devolver un error 500 si ocurre una excepción al actualizar
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            Cliente cliente = await _dbContext.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            // Eliminar todas las cuentas de inversión asociadas al cliente
            var cuentasInversion = await _dbContext.CuentaInversions.Where(c => c.IdCliente == id).ToListAsync();
            _dbContext.CuentaInversions.RemoveRange(cuentasInversion);

            _dbContext.Clientes.Remove(cliente);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, "ok");
        }


    }
}
