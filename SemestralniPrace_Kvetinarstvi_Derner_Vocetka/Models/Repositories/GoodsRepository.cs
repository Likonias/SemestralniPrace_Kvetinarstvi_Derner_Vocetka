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

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories
{
    //TODO ALSO SKIPPED, BECAUSE NOT USED
    public class GoodsRepository : IRepository<Goods>
    {
        public ObservableCollection<Goods> Goods { get; set; }
        private OracleDbUtil dbUtil;
        
        public GoodsRepository()
        {
            Goods = new ObservableCollection<Goods>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<Goods> GetById(Int32 id)
        {
            string command = $"SELECT * FROM zbozi WHERE ID_ZBOZI = {id}";
            var dataTable = await dbUtil.ExecuteCommandAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var goods = new Goods(
                Convert.ToInt32(row["ID_ZBOZI"]),
                row["NAZEV"].ToString(),
                Convert.ToInt32(row["CENA"]),
                Convert.ToChar(row["TYP"]),
                Convert.ToInt32(row["SKLAD"]),
                null
            );
            
            return (Goods)Convert.ChangeType(goods, typeof(Goods));
        }

        public async Task GetAll()
        {
            Goods.Clear();
            string command = "SELECT * FROM zbozi";
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var goods = new Goods(
                    Convert.ToInt32(row["ID_ZBOZI"]),
                    row["NAZEV"].ToString(),
                    Convert.ToInt32(row["CENA"]),
                    Convert.ToChar(row["TYP"]),
                    Convert.ToInt32(row["SKLAD"]),
                    null
                );
                Goods.Add(goods);
            }
        }

        public async Task Add(Goods entity)
        {
            var parameters = new Dictionary<string, object>
            { 
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "TYP", entity.Type },
                { "SKLAD", entity.Warehouse }
            };
            await dbUtil.ExecuteStoredProcedureAsync("addzbozi", parameters);
        }

        public async Task Update(Goods entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZBOZI", entity.IdGoods },
                { "NAZEV", entity.Name },
                { "CENA", entity.Price },
                { "TYP", entity.Type },
                { "SKLAD", entity.Warehouse }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatezbozi", parameters);
        }

        public async Task Delete(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZBOZI", id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletezbozi", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            var dataTable = new DataTable();

            dataTable.Columns.Add("NAZEV", typeof(string));
            dataTable.Columns.Add("CENA", typeof(int));
            dataTable.Columns.Add("TYP", typeof(byte));
            dataTable.Columns.Add("SKLAD", typeof(int));

            foreach (var goods in Goods)
            { 
                dataTable.Rows.Add(goods.Name, goods.Price, goods.Type, goods.Warehouse);
            }
            
            return dataTable;
        }

        private List<Goods> GetGoods()
        {
            Task.Run(async () => await GetAll()).Wait();
            var goods = Goods.ToList();

            return goods;
        }
    }
}