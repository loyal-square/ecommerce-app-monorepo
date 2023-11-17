using Microsoft.AspNetCore.Mvc;
using MonolithServer.Database;
using MonolithServer.Models;

namespace MonolithServer.Controllers;

[ApiController]
[Route("api/Profile")]
public class ProfileController: ControllerBase
{
    [HttpPost]
    [Route("create")]
    public async Task CreateProfile(Profile profile)
    {
        //Ensure profile's username (unique identifier) does not already exist
    }
}