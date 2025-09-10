using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Сотрудники
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IRepository<Employee> _employeeRepository;
    private readonly IRepository<Role> _roleRepository;

    public EmployeesController(IRepository<Employee> employeeRepository, IRepository<Role> roleRepository)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// Получить данные всех сотрудников
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        var employeesModelList = employees.Select(x =>
            new EmployeeShortResponse(x.Id, x.FullName, x.Email)).ToList();

        return employeesModelList;
    }

    /// <summary>
    /// Получить данные сотрудника по Id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            return NotFound();

        var employeeModel = new EmployeeResponse(employee.Id, employee.FullName, employee.Email, employee.Roles.Select(x => new RoleItemResponse()
        {
            Name = x.Name,
            Description = x.Description
        }).ToList(), employee.AppliedPromocodesCount);

        return employeeModel;
    }

    /// <summary>
    /// Добавить сотрудника
    /// </summary>
    /// <param name="employeeDto">Данные сотрудника</param>
    /// <returns></returns>
    [HttpPost()]
    public async Task<ActionResult<Guid>> AddEmployeeAsync(AddEmployeeDto employeeDto)
    {
        var roles = new List<Role>();

        foreach (var roleId in employeeDto.RolesId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);

            if (role == null)            
                return NotFound($"Роль с Id={roleId} не найдена");            

            roles.Add(role);
        }

        var employee = new Employee
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Email = employeeDto.Email,
            Roles = roles,
            AppliedPromocodesCount = employeeDto.AppliedPromocodesCount
        };

        employee.Id = await _employeeRepository.AddAsync(employee);

        return Ok(employee.Id);
    }

    /// <summary>
    /// Обновить сотрудника
    /// </summary>
    /// <param name="id">id сотрудника</param>
    /// <param name="employeeDto">Данные сотрудника</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto employeeDto)
    {
        var roles = new List<Role>();

        foreach (var roleId in employeeDto.RolesId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);

            if (role == null)            
                return NotFound($"Роль с Id={roleId} не найдена");            

            roles.Add(role);
        }

        var employeeExisted = await _employeeRepository.GetByIdAsync(id);

        employeeExisted.FirstName = employeeDto.FirstName;
        employeeExisted.LastName = employeeDto.LastName;
        employeeExisted.Email = employeeDto.Email;
        employeeExisted.Roles = roles;
        employeeExisted.AppliedPromocodesCount = employeeDto.AppliedPromocodesCount;

        await _employeeRepository.UpdateAsync(id, employeeExisted);

        return NoContent();
    }

    /// <summary>
    /// Удалить сотрудника
    /// </summary>
    /// <param name="id">ИД сотрудника</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> DeleteEmployeeAsync(Guid id)
    {
        await _employeeRepository.DeleteAsync(id);

        return NoContent();
    }
}