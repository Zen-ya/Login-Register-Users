using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;



 [ApiController]
 [Route("api/[controller]")]
 public class UsersControllers : ControllerBase
 {
     [HttpGet("all")]
     [ProducesResponseType(StatusCodes.Status204NoContent)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     [ProducesResponseType(StatusCodes.Status200OK)]
     public ActionResult<List<Users>> GetAll()
     {
         try
         {
             List<Users> users = DBServices.GetUsers();
             return Ok(users);
         }
         catch (Exception e)
         {
             return BadRequest(e.Message);
         }
     }

    [HttpGet("{id:int:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Users))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUserById(int id)
    {
        try
        {
            Users user = DBServices.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with id = {id} was not found." });
            }
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }


    [HttpPost("create")]
     [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Users))]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
     public IActionResult CreateUser([FromBody] Users newUser)
     {
         bool result = DBServices.CreateUser(newUser);
         if (result)
         {
             return Ok(new { message = "User created successfully." });
         }
         else
         {
             return BadRequest(new { message = "Failed to create user." });
         }
     }



     [HttpPut("{id}")]
     [ProducesResponseType(StatusCodes.Status204NoContent)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
     public IActionResult UpdateUser(int id, [FromBody] Users updatedUser)
     {
         bool result = DBServices.UpdateUser(id, updatedUser);
         if (result)
         {
             return Ok(new { message = "User updated successfully." });
         }
         else
         {
             return NotFound(new { message = "User not found." });
         }
     }

     [HttpDelete("{id}")]
     [ProducesResponseType(StatusCodes.Status204NoContent)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
     public IActionResult DeleteUser(int id)
     {
         bool result = DBServices.DeleteUserById(id);
         if (result)
         {
             return Ok(new { message = "User deleted successfully." });
         }
         else
         {
             return NotFound(new { message = "User not found." });
         }
     }

     [HttpPost("login")]
     [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Users))]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     public IActionResult Login([FromBody] LoginData ld)
     {
         try
         {
             if (ld == null || string.IsNullOrEmpty(ld.UserName) || string.IsNullOrEmpty(ld.Password))
             {
                 return BadRequest("Invalid login request.");
             }

             Users usr = DBServices.Login(ld.UserName, ld.Password);
             if (usr != null)
             {
                 return Ok(usr);
             }
             else
             {
                 return Unauthorized("Invalid username or password.");
             }
         }
         catch (Exception e)
         {
             return BadRequest(e.Message);
         }
     }

 }