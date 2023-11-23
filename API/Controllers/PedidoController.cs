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
    public class PedidoController :ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PedidoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> Get()
        {
            var pedidos = await _unitOfWork.Pedidos.GetAllAsync();
            return _mapper.Map<List<PedidoDto>>(pedidos);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoDto>> Get(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return _mapper.Map<PedidoDto>(cliente);
        }
        
      //Consulta
    /*   [HttpGet("Consulta1")]
      public async Task<IActionResult> GetConsulta1()
      {
        var consulta = await _unitOfWork.Clientes.getConsulta1().ConfigureAwait(false);
        return Ok(consulta);
      }
 */

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PedidoDto>> Post(PedidoDto pedidoDto)
        {
            var pedidos = _mapper.Map<Pedido>(pedidoDto);
            _unitOfWork.Pedidos.Add(pedidos);
            await _unitOfWork.SaveAsync();
            if (pedidos == null)
            {
                return BadRequest();
            }
            pedidoDto.CodigoPedido = pedidos.CodigoPedido;
            return CreatedAtAction(nameof(Post), new { id = pedidoDto.CodigoPedido }, pedidoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoDto>> Put(int id, [FromBody] PedidoDto pedidoDto)
        {
            if (pedidoDto.CodigoPedido == 0)
            {
                pedidoDto.CodigoPedido = id;
            }
            if (pedidoDto.CodigoPedido!= id)
            {
                return NotFound();
            }
            var cliente = _mapper.Map<Cliente>(pedidoDto);
            pedidoDto.CodigoCliente = cliente.CodigoCliente;
            _unitOfWork.Clientes.Update(cliente);
            await _unitOfWork.SaveAsync();
            return pedidoDto;
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

