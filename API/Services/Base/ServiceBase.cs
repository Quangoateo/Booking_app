using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookingApp.Constants;
using BookingApp.Data;
using BookingApp.DTO;
using BookingApp.Helpers;
using BookingApp.Models;
using Microsoft.EntityFrameworkCore;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using Syncfusion.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

/// <summary>
/// Represents the base service interface for CRUD operations on entities.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <typeparam name="TDto">The DTO (Data Transfer Object) type.</typeparam>
public interface IServiceBase<T, TDto>
{
    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="model">The DTO model representing the entity to be added.</param>
    /// <returns>An operation result indicating the success or failure of the operation.</returns>
    /// /// <remarks>Only override this method when there are logic fields other than Guid or Status. </remarks>
    Task<OperationResult> AddAsync(TDto model);
    Task<OperationResult> AddAsync(T model);
    /// <summary>
    /// Adds a range of entities asynchronously.
    /// </summary>
    /// <param name="model">The list of DTO models representing the entities to be added.</param>
    /// <returns>An operation result indicating the success or failure of the operation.</returns>
    /// <remarks>Only override this method when there are logic fields other than Guid or Status. </remarks>
    Task<OperationResult> AddRangeAsync(List<TDto> model);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="model">The DTO model representing the entity to be updated.</param>
    /// <returns>An operation result indicating the success or failure of the operation.</returns>
    Task<OperationResult> UpdateAsync(TDto model);

    /// <summary>
    /// Updates a range of existing entities asynchronously.
    /// </summary>
    /// <param name="model">The list of DTO models representing the entities to be updated.</param>
    /// <returns>An operation result indicating the success or failure of the operation.</returns>
    Task<OperationResult> UpdateRangeAsync(List<TDto> model);

    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="id">The ID of the entity to be deleted.</param>
    /// <returns>An operation result indicating the success or failure of the operation.</returns>
    /// <remarks>Consider using the <see cref="DisableAsync"/> method instead, as using this method may cause problems in the database.</remarks>
    Task<OperationResult> DeleteAsync(int id);

     /// <summary>
    /// Turn off the status of an entity asynchronously.
    /// </summary>
    /// <param name="id">The ID of the entity to be Disabled.</param>
    /// <returns>An operation result indicating the success or failure of the operation.</returns>
    /// <remarks>Object still remain on the Database, but considered "delete" </remarks>
    Task<OperationResult> DisableAsync(int id);

    /// <summary>
    /// Gets all entities asynchronously.
    /// </summary>
    /// <returns>A list of DTO models representing all the entities.</returns>
    Task<List<TDto>> GetAllAsync();

    /// <summary>
    /// Gets entities with pagination asynchronously. Include object with status = true
    /// </summary>
    /// <param name="param">The pagination parameters.</param>
    /// <returns>A paged list of DTO models representing the entities.</returns>
    /// <remarks>Override this method to add custom sorting and filtering logic.</remarks>
    Task<PagedList<TDto>> GetWithPaginationsAsync(PaginationParams param);

    /// <summary>
    /// Searches for entities asynchronously based on the specified text.
    /// </summary>
    /// <param name="param">The pagination parameters.</param>
    /// <param name="text">The text to search for.</param>
    /// <returns>A paged list of DTO models representing the matching entities.</returns>
    Task<PagedList<TDto>> SearchAsync(PaginationParams param, object text);

    /// <summary>
    /// Gets an entity by ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The DTO model representing the entity.</returns>
    TDto GetByID(int id);

    /// <summary>
    /// Gets an entity by ID asynchronously.\
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The DTO model representing the entity.</returns>
    Task<TDto> GetByIDAsync(int id);

    /// <summary>
    /// Gets the data for a dropdown list asynchronously. 
    /// </summary>
    /// <param name="data">The data manager object containing filtering, sorting, and paging parameters.</param>
    /// <returns>The data for the dropdown list.</returns>
    Task<object> GetDataDropdownlist(DataManager data);
}
namespace BookingApp.Services.Base
{
    public interface IServiceBase<T, TDto>
    {
        Task<OperationResult> AddAsync(TDto model);
        Task<OperationResult> AddAsync(T model);
        Task<OperationResult> AddRangeAsync(List<TDto> model);


        Task<OperationResult> UpdateAsync(TDto model);
        Task<OperationResult> UpdateRangeAsync(List<TDto> model);

        Task<OperationResult> DeleteAsync(int id);
        Task<OperationResult> DisableAsync(int id);

        Task<List<TDto>> GetAllAsync();

        Task<PagedList<TDto>> GetWithPaginationsAsync(PaginationParams param);

        Task<PagedList<TDto>> SearchAsync(PaginationParams param, object text);
        TDto GetByID(int id);
        Task<TDto> GetByIDAsync(int id);
        Task<object> GetDataDropdownlist(DataManager data);
    }
    public class ServiceBase<T, TDto> : IServiceBase<T, TDto> where T : class
    {
        public OperationResult operationResult;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase<T> _repo;
        private readonly MapperConfiguration _configMapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceBase(
            IRepositoryBase<T> repo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _mapper = mapper;
            _repo = repo;
            _configMapper = configMapper;
            _unitOfWork = unitOfWork;
        }
        public virtual async Task<OperationResult> AddAsync(TDto model)
        {
            var item = _mapper.Map<T>(model);
            PropertyInfo propGuid = item.GetType().GetProperty("Guid");
            propGuid.SetValue(item, Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper());
            PropertyInfo propStatus = item.GetType().GetProperty("Status");
            propStatus.SetValue(item, true);
            _repo.Add(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public virtual async Task<OperationResult> AddAsync(T model)
        {
            var item = _mapper.Map<T>(model);
            PropertyInfo propGuid = item.GetType().GetProperty("Guid");
            propGuid.SetValue(item, Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper());
            PropertyInfo propStatus = item.GetType().GetProperty("Status");
            propStatus.SetValue(item, true);
            _repo.Add(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public virtual async Task<OperationResult> AddRangeAsync(List<TDto> model)
        {
            var item = _mapper.Map<List<T>>(model);
            foreach (var i in item)
            {
                PropertyInfo propGuid = i.GetType().GetProperty("Guid");
                propGuid.SetValue(i, Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper());
                PropertyInfo propStatus = i.GetType().GetProperty("Status");
                propStatus.SetValue(i, true);
            }
            _repo.AddRange(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public virtual async Task<OperationResult> DeleteAsync(int id)
        {
            var item = _repo.FindByID(id);
            _repo.Remove(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public virtual async Task<OperationResult> DisableAsync(int id)
        {
            var item = _repo.FindByID(id);
            PropertyInfo propStatus = item.GetType().GetProperty("Status");
            propStatus.SetValue(item, false);
            _repo.Update(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DisableSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public virtual async Task<List<TDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<TDto>(_configMapper).ToListAsync();
        }

        public virtual TDto GetByID(int id)
        {
            return _mapper.Map<T, TDto>(_repo.FindByID(id));
        }

        public virtual async Task<TDto> GetByIDAsync(int id)
        {
            return _mapper.Map<T, TDto>(await _repo.FindByIDAsync(id));
        }
        public virtual async Task<PagedList<TDto>> GetWithPaginationsAsync(PaginationParams param)
        {
            try
            {

                // Tạo một biểu thức tham chiếu tới thuộc tính "ID" của đối tượng TDto
                var parameter = Expression.Parameter(typeof(TDto), "x");
                var property = Expression.Property(parameter, "ID");

                // Chuyển đổi kiểu trả về từ int sang object
                var conversion = Expression.Convert(property, typeof(object));

                var lambda = Expression.Lambda<Func<TDto, object>>(conversion, parameter);

                // Tạo biểu thức sắp xếp sử dụng biểu thức lambda đã tạo
                var orderByExp = Expression.Lambda<Func<TDto, object>>(conversion, lambda.Parameters);

                var lists = _repo.FindAll().ProjectTo<TDto>(_configMapper).OrderByDescending(orderByExp);

                return await PagedList<TDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public virtual async Task<PagedList<TDto>> SearchAsync(PaginationParams param, object text)
        {
            var lists = _repo.FindAll().ProjectTo<TDto>(_configMapper)
          .Where(x => x.GetType().GetProperty("Name").GetValue(x) == text)
          .OrderByDescending(x => x.GetType().GetProperty("ID"));
            return await PagedList<TDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        public virtual async Task<OperationResult> UpdateAsync(TDto model)
        {
            var item = _mapper.Map<T>(model);
            _repo.Update(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public virtual async Task<OperationResult> UpdateRangeAsync(List<TDto> model)
        {
            var item = _mapper.Map<List<T>>(model);
            _repo.UpdateRange(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public virtual async Task<object> GetDataDropdownlist(DataManager data)
        {
            var datasource = _repo.FindAll().AsQueryable();
            var count = await datasource.CountAsync();
            if (data.Where != null) // for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = await datasource.CountAsync();
            if (data.Skip >= 0)//for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return await datasource.ToListAsync();
        }

    }
}
