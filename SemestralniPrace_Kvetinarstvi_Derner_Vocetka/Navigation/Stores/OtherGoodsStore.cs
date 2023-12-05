using System;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class OtherGoodsStore
    {
        private Action otherGoodsChanged;
        private OtherGoods otherGoods;

        public OtherGoods OtherGoods
        {
            get { return otherGoods; }
            set
            {
                otherGoods = value;
                OnCurrentOtherGoodsChanged();
            }
        }

        public void OnCurrentOtherGoodsChanged()
        {
            otherGoodsChanged?.Invoke();
        }
    }
}
