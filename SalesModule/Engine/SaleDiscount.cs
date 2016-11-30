using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SalesModule
{
    [Guid(SaleDiscount.InterfaceId)]
    public interface ISaleDiscount
    {
        int SaleID { get; }
        int ID { get; }
        string Title { get; }
        double Quantity { get; }
        double Discount { get; }

        bool IsContainProduct(string pluno);
    }

    [Guid(ClassId), ClassInterface(ClassInterfaceType.None)]
    public class SaleDiscount : ISaleDiscount
    {
        #region COM
#if COM_INTEROP_ENABLED
        public const string ClassId = "6cd60528-bc08-4c6a-9a6a-7eadecf912c2";
        public const string InterfaceId = "488d5b5f-049a-4cb7-be1c-dfef6435b317";

        // These routines perform the additional COM registration needed by ActiveX controls
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction]
        private static void Register(System.Type t)
        {
            ComRegistration.RegisterControl(t);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction]
        private static void Unregister(System.Type t)
        {
            ComRegistration.UnregisterControl(t);
        }

#endif
        #endregion

        internal static int _salesID;

        public int SaleID { get; private set; }
        public int ID { get; private set; }
        public string Title { get; private set; }
        public double Quantity { get; private set; }
        public double Discount { get; private set; }
        public List<string> Plunos { get; private set; }

        public SaleDiscount() : this(-1, "", 0, 0, new List<string>()) { }
        internal SaleDiscount(int saleID, string title, double QTY, double dis, List<string> plunos)
        {
            SaleID = saleID;
            ID = -1;
            Title = title;
            Quantity = QTY;
            Discount = dis;
            Plunos = plunos;
        }
        static SaleDiscount()
        {
            ResetCounter();
        }
        internal static void ResetCounter()
        {
            _salesID = 0;
        }
        internal void SetID()
        {
            ID = ++_salesID;
        }
        internal void SetID(int id)
        {
            ID = id;
        }

        public override bool Equals(object obj)
        {
            return obj is SaleDiscount && (SaleDiscount)obj == this;
        }
        public static bool operator ==(SaleDiscount sd1, SaleDiscount sd2)
        {
            return sd1.Title == sd2.Title &&
                sd1.Discount == sd2.Discount &&
                sd1.Quantity == sd2.Quantity;
        }
        public static bool operator !=(SaleDiscount sd1, SaleDiscount sd2)
        {
            return !(sd1 == sd2);
        }
        public override int GetHashCode()
        {
            return ID;
        }

        internal bool Add(SaleDiscount sd)
        {
            if (Title == sd.Title && Discount == sd.Discount)
            {
                Quantity += sd.Quantity;
                return true;
            }
            return false;
        }
        public bool IsContainProduct(string pluno)
        {
            return Plunos.Exists(p => p == pluno);
        }
    }

}
