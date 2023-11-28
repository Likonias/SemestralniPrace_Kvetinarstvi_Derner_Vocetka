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
    public class OrderGoodsRepository : IRepository<OrderGoods>
    {
        //TODO jak implemetujeme OrderGoods?
        
        public ObservableCollection<OrderGoods> OrderGoods { get; set; }
        private OracleDbUtil dbUtil;
        
        public OrderGoodsRepository()
        {
            OrderGoods = new ObservableCollection<OrderGoods>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<OrderGoods> GetById(Int32 id)
        {
            throw new NotImplementedException();
        }

        public async Task GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task Add(OrderGoods entity)
        {
            throw new NotImplementedException();
        }

        public async Task Update(OrderGoods entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(OrderGoods entity)
        {
            throw new NotImplementedException();
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            throw new NotImplementedException();
        }
    }
}