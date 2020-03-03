using System.Threading.Tasks;
using Application.Photos;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PhotosController:BaseController
    {
        //FromForm tells it where to look (we use form-data in postman)
        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm] IFormFile File)
        {
            return await Mediator.Send(new Add.Command{File=File});
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            return await Mediator.Send(new Delete.Command{Id=id});
        }

        [HttpPost("{id}/setmain")]
        public async Task<ActionResult<Unit>> SetMain(string id)
        {
            return await Mediator.Send(new SetMain.Command{Id=id});
        }

    }
}