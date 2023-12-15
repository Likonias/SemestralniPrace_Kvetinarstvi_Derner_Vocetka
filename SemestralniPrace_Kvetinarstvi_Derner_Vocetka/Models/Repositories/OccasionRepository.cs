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

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories{
    public class OccasionRepository
    { 
        public ObservableCollection<Occasion> Occasions { get; set; }
        private OracleDbUtil dbUtil;

        public OccasionRepository()
        {
            Occasions = new ObservableCollection<Occasion>();
            dbUtil = new OracleDbUtil();
        }

        public async Task GetAll()
        {
            Occasions.Clear();
            string command = "GET_ALL_PRILEZITOSTI";
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command);
            
            foreach (DataRow row in dataTable.Rows)
            {
                var occasion = new Occasion(
                    Convert.ToInt32(row["ID_PRILEZITOST"]),
                    (OccasionTypeEnum)(row["DRUH_PRILEZITOSTI"] != DBNull.Value ? Enum.Parse(typeof(OccasionTypeEnum), row["OccasionType"].ToString()) : null),
                    Convert.ToInt32(row["OBJEDNAVKY_ID_OBJEDNAVKY"]));
                Occasions.Add(occasion);
            }
        }

        public async Task Add(Occasion entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"DRUH_PRILEZITOSTI", entity.OccasionType.ToString()}
            };
            await dbUtil.ExecuteStoredProcedureAsync("AddData.addprilezitosti", parameters);
        }

        public async Task Update(Occasion entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_PRILEZITOST", entity.Id},
                {"DRUH_PRILEZITOSTI", entity.OccasionType.ToString()}
            };
            await dbUtil.ExecuteStoredProcedureAsync("updateprilezitosti", parameters);
        }

        public async Task Delete(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_PRILEZITOST", id}
            };
            await dbUtil.ExecuteStoredProcedureAsync("deleteprilezitosti", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            var dataTable = new DataTable();
            
            dataTable.Columns.Add("ID_PRILEZITOST", typeof(int));
            dataTable.Columns.Add("DRUH_PRILEZITOSTI", typeof(string));
            
            foreach (var occasion in Occasions)
            {
                dataTable.Rows.Add(occasion.Id, occasion.OccasionType.ToString());
            }
            
            return dataTable;
        }

        public List<Occasion> GetOccasions()
        {
            Task.Run(async () => await GetAll()).Wait();
            var ocasions = Occasions.ToList();

            return ocasions;
        }
    }
}