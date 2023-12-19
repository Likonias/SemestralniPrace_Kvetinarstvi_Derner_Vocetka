using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Components
{
    enum ComboBoxTableNamesEnum
    {

        [Description("Addresses")]
        Addresses,

        [Description("Flowers")]
        Flowers,

        [Description("Other Goods")]
        OtherGoods,

        [Description("Customers")]
        Customers,

        [Description("Employees")]
        Employees,

        [Description("Orders")]
        Orders,

        [Description("Address Types")]
        AddressTypes,

        [Description("Delivery Methods")]
        DeliveryMethods,

        [Description("Deliveries")]
        Deliveries,

        [Description("In Person Pickups")]
        InPersonPickups,

        [Description("Invoices")]
        Invoices,

        [Description("Occasions")]
        Occasions,

        [Description("System Catalog")]
        SystemCatalog,

        [Description("Database History")]
        History,

        [Description("Other Goods Preview")]
        UserOther,

        [Description("Flowers Preview")]
        UserFlower

    }
}
