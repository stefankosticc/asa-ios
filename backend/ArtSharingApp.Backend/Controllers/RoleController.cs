using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using ArtSharingApp.Backend.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api")]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpPost("role")]
    public async Task<IActionResult> AddRole([FromBody] RoleRequestDTO roleDto)
    {
        await _roleService.AddRoleAsync(roleDto);
        return Ok(new {message = "Role added successfully"});
    }

    [AllowAnonymous]
    [HttpGet("roles")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _roleService.GetAllAsync());
    }

    [HttpGet("role/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = await _roleService.GetByIdAsync(id);
        return Ok(role);
    }

    [HttpPut("role/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoleRequestDTO roleDto)
    {
        await _roleService.UpdateAsync(id, roleDto);
        return Ok(new {message = "Role updated successfully"});
    }

    [HttpDelete("role/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _roleService.DeleteAsync(id);
        return Ok(new {message = "Role deleted successfully"});
    }
}
