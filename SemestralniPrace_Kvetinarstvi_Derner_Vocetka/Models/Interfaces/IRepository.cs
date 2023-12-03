using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetById(int id);
        Task GetAll();
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task<DataTable> ConvertToDataTable();

    }
}
