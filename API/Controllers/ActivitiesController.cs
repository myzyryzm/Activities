using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//receive HTTP requests and send back HTTP responses
//dotnet ef database drop -p Persistence/ -s API/ to reset database
namespace API.Controllers {
    //boxing makes a copy of the value type
    //boxing is converting a value type to an object by putting it on the garbage collected heap; unboxing is converting an object to a value type (i.e. taking it off the heap and putting it on the stack)
    //generics remove the possibility of boxing and unboxing
    //boxing -> putting value type on heap; unboxing -> taking obj off heap and putting it on stack
    //value types => struct, int, bool, char, float, enum ; contains an instance of that type
    //when you pass these into functions they do not change
    //reference => class, interface, string, obj ; contains a reference to an instance of the type
    //if you copy a reference variable and then change the copy, the original reference will be mutated too
    public class ActivitiesController : BaseController {

        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> List () {
            return await Mediator.Send (new List.Query ());
        }

        [HttpGet ("{id}")]
        [Authorize]
        public async Task<ActionResult<ActivityDto>> Details (Guid id) {
            //_mediator is sending a new Details Query class with a specified Id equal to the Id we send in the api request
            //mediator is added to the the services collection so it is accessible from all classes
            //we added an assembly of handlers (of type IRequestHandler) when we added the mediator to the services collection
            //when we send we passed in a class of IRequest
            return await Mediator.Send (new Details.Query { Id = id });
        }
        //unit is just like an empty object
        //just naming the command class tells the class to look for the properties that is being set up with the request 
        //we send up the api request through our command
        //apicontroller uses binding source inference
        //fluent validation (acts as a middleware)
        //command => validate command => handler logic
        //<T> after a function name specifies the type of arguments the function is going to take
        [HttpPost]
        public async Task<ActionResult<Unit>> Create (Create.Command command) {
            return await Mediator.Send (command);
        }

        [HttpPut ("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult<Unit>> Edit (Guid id, Edit.Command command) 
        {
            command.Id = id;
            return await Mediator.Send (command);
        }

        [HttpDelete ("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult<Unit>> Delete (Guid id) {
            return await Mediator.Send (new Delete.Command { Id = id });
        }

        [HttpPost ("{id}/attend")]
        public async Task<ActionResult<Unit>> Attend (Guid id) {
            return await Mediator.Send (new Attend.Command { Id = id });
        }

        [HttpDelete ("{id}/attend")]
        public async Task<ActionResult<Unit>> Unattend (Guid id) {
            return await Mediator.Send (new Unattend.Command { Id = id });
        }
    }
}