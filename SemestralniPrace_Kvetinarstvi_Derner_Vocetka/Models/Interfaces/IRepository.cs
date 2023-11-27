using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces
{
    public interface IRepository<T>
    {
        //TODO všechny repositories k Entities
        Task<T> GetById(int id);
        Task GetAll();
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }
}
