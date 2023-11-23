using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmpleadoController :ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmpleadoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> Get()
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync();
            return _mapper.Map<List<EmpleadoDto>>(clientes);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmpleadoDto>> Get(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return _mapper.Map<EmpleadoDto>(cliente);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClienteDto>> Post(EmpleadoDto empleadoDto)
        {
            var clientes = _mapper.Map<Cliente>(empleadoDto);
            _unitOfWork.Clientes.Add(clientes);
            await _unitOfWork.SaveAsync();
            if (clientes == null)
            {
                return BadRequest();
            }
            empleadoDto.CodigoEmpleado = empleadoDto.CodigoEmpleado;
            return CreatedAtAction(nameof(Post), new { id = empleadoDto.CodigoEmpleado }, empleadoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmpleadoDto>> Put(int id, [FromBody] EmpleadoDto empleadoDto)
        {
            if (empleadoDto.CodigoEmpleado == 0)
            {
                empleadoDto.CodigoEmpleado = id;
            }
            if (empleadoDto.CodigoEmpleado != id)
            {
                return NotFound();
            }
            var cliente = _mapper.Map<Cliente>(empleadoDto);
            empleadoDto.CodigoEmpleado = cliente.CodigoCliente;
            _unitOfWork.Clientes.Update(cliente);
            await _unitOfWork.SaveAsync();
            return empleadoDto;
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var clientes = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (clientes == null)
            {
                return NotFound();
            }
            _unitOfWork.Clientes.Remove(clientes);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
        
    }
}