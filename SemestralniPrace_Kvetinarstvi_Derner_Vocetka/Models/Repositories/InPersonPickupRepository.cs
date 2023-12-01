using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories
{
    public class InPersonPickupRepository : IRepository<InPersonPickup>
    {
        public ObservableCollection<InPersonPickup> InPersonPickups { get; set; }
        private OracleDbUtil dbUtil;
      
        public InPersonPickupRepository()
        {
            InPersonPickups = new ObservableCollection<InPersonPickup>();
            dbUtil = new OracleDbUtil();
        }
      
        public async Task<InPersonPickup> GetById(Int32 id)
        {
            string command = $"SELECT * FROM osobne" +
                             $"JOIN ZPUSOBY_PREVZETI on ZPUSOBY_PREVZETI.id_zpusob_prevzeti = osobne.id_zpusob_prevzeti" +
                             $"WHERE ID_OSOBNE = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;
          
            var row = dataTable.Rows[0];
            var inPersonPickup = new InPersonPickup(
                Convert.ToInt32(row["id_zpusob_prevzeti"]),
                Convert.ToDateTime(row["DATUM_VYDANI"]),
                Convert.ToInt32(row["OBJEDNAVKY_ID_OBJEDNAVKA"]),
                (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["TYP"].ToString()),
                Convert.ToInt32(row["ID_OSOBNE"]),
                row["CAS"].ToString()
            );
          
            return (InPersonPickup)Convert.ChangeType(inPersonPickup, typeof(InPersonPickup));
        }

        public async Task GetAll()
        {
            string command = $"SELECT * FROM osobni_vyzvednuti" +
                             $"JOIN ZPUSOBY_PREVZETI on ZPUSOBY_PREVZETI.id_zpusob_prevzeti = osobne.id_zpusob_prevzeti";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var inPersonPickup = new InPersonPickup(
                    Convert.ToInt32(row["id_zpusob_prevzeti"]),
                    Convert.ToDateTime(row["DATUM_VYDANI"]),
                    Convert.ToInt32(row["OBJEDNAVKY_ID_OBJEDNAVKA"]),
                    (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["TYP"].ToString()),
                    Convert.ToInt32(row["ID_OSOBNE"]),
                    row["CAS"].ToString()
                );

                InPersonPickups.Add(inPersonPickup);
            }
        }

        public async Task Add(InPersonPickup entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"DATUM_VYDANI", entity.WarehouseReleaseDate },
                {"OBJEDNAVKY_ID_OBJEDNAVKA" , entity.IdOrder},
                {"TYP", entity.Method },
                { "cas", entity.Time }
            };
          
            await dbUtil.ExecuteStoredProcedureAsync("addosobni_vyzvednuti", parameters);
        }

        public async Task Update(InPersonPickup entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "id_zpusob_prevzeti", entity.IdDeliveryMethod },
                {"DATUM_VYDANI", entity.WarehouseReleaseDate },
                {"OBJEDNAVKY_ID_OBJEDNAVKA" , entity.IdOrder},
                {"TYP", entity.Method },
                { "id_osobne", entity.IdPickup },
                { "cas", entity.Time }
            };
          
            await dbUtil.ExecuteStoredProcedureAsync("updateosobni_vyzvednuti", parameters);
          
        }

        public async Task Delete(InPersonPickup entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "id_zpusob_prevzeti", entity.IdDeliveryMethod },
                { "id_osobne", entity.IdPickup }
            };
          
            await dbUtil.ExecuteStoredProcedureAsync("deleteosobni_vyzvednuti", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            var dataTable = new DataTable();
          
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Čas", typeof(string));
          
            foreach (var inPersonPickup in InPersonPickups)
            {
                dataTable.Rows.Add(inPersonPickup.IdPickup, inPersonPickup.Time);
            }
          
            return dataTable;
        }
    }
}