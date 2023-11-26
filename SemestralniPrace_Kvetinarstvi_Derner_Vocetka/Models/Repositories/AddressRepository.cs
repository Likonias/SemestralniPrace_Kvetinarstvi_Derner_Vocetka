using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories
{
    public class AddressRepository : IRepository<Address>
    {

        public ObservableCollection<Address> Addresses { get; set; }

        private OracleDbUtil dbUtil;

        public AddressRepository() 
        {
            Addresses = new ObservableCollection<Address>();
            dbUtil = new OracleDbUtil();
        }

        public async Task Add(Address entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "street", entity.Street },
                { "streetNumber", entity.StreetNumber },
                { "city", entity.City },
                { "zip", entity.Zip },
                
            };
            await dbUtil.ExecuteStoredProcedureAsync("addAddress", parameters);
        }

        public async Task Delete(Address entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Address>> GetAll()
        {
            string command = "ddd";
            //DataTable dataTable = await dbUtil.ExecuteQuery(command);

            throw new NotImplementedException();
        }

        public async Task<Address> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Address entity)
        {
            throw new NotImplementedException();
        }
    }
}
