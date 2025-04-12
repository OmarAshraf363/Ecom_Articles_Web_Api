using AutoMapper;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.ApI.Controllers
{
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("not-found")]
        public async Task<ActionResult> GetNotFound()
        {
            var product = await _unitOfWork.ArticleRepository.GetByIdAsync(0);
            if (product == null) return NotFound();
            return Ok(product);
        }
        [HttpGet("server-error")]
        public async Task<ActionResult> GetServerError()
        {
            var product = await _unitOfWork.ArticleRepository.GetByIdAsync(0);
            product.Description = "";
            return Ok(product);
        }
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }
        [HttpGet("bad-request/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }


    }
}
