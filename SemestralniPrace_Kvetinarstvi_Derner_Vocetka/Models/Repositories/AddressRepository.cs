using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

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
                { "ULICE", entity.Street },
                { "CISLO_POPISNE", entity.StreetNumber },
                { "MESTO", entity.City },
                { "PSC", entity.Zip },
                { "ZAMESTNANCI_ID_ZAMESTNANEC", entity.EmployeeId },
                { "ZAKAZNICI_ID_ZAKAZNIK", entity.CustomerId },
                { "DRUHY_ADRES_ID_DRUH_ADRESY", entity.AddressType },
            };
            await dbUtil.ExecuteStoredProcedureAsync("addAddress", parameters);

            //procedure?

            //CREATE OR REPLACE PROCEDURE addAddress(
            //    ULICE IN VARCHAR2,
            //    CISLO_POPISNE IN VARCHAR2,
            //    MESTO IN VARCHAR2,
            //    PSC IN VARCHAR2,
            //    ZAMESTNANCI_ID_ZAMESTNANEC IN NUMBER,
            //    ZAKAZNICI_ID_ZAKAZNIK IN NUMBER,
            //    DRUHY_ADRES_ID_DRUH_ADRESY IN NUMBER
            //            )
            //IS
            //BEGIN
            //    INSERT INTO Addresses(
            //        Street,
            //        StreetNumber,
            //        City,
            //        Zip,
            //        EmployeeId,
            //        CustomerId,
            //        AddressType
            //    ) VALUES(
            //        ULICE,
            //        CISLO_POPISNE,
            //        MESTO,
            //        PSC,
            //        ZAMESTNANCI_ID_ZAMESTNANEC,
            //        ZAKAZNICI_ID_ZAKAZNIK,
            //        DRUHY_ADRES_ID_DRUH_ADRESY
            //    );
            //            COMMIT;
            //            END;
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
