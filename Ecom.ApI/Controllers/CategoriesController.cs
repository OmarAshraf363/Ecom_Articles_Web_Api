using AutoMapper;
using Ecom.ApI.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.ApI.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork unitOfWork,IMapper mapper) : base(unitOfWork,mapper)
        {
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepo.GetAllAsync();
                if(categories is null)
                {
                    return BadRequest(new ResponseApi(statusCode:400));
                }
                return Ok(categories.Select(e => new
                {
                    id=e.Id,
                    name = e.Name,
                    description = e.Description,
                    createdAt = e.CreatedAt,
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }


        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepo.GetByIdAsync(id);
                if (category is null)
                {
                    return BadRequest(new ResponseApi(statusCode: 400));
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(CategoryDTO categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                await _unitOfWork.CategoryRepo.AddAsync(category);
                return Ok(new ResponseApi(200,"Item Created Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdatedCategoryDTO categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                await _unitOfWork.CategoryRepo.UpdateAsync(category);
                return Ok(new ResponseApi(200, "Item Updated Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepo.GetByIdAsync(id);
                if (category is null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                await _unitOfWork.CategoryRepo.DeleteAsync(id);
                return Ok(new ResponseApi(200, "Item Deleted Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
