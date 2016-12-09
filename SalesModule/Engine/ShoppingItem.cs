using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SalesModule.Models;

namespace SalesModule
{
    [Guid(ShoppingItem.InterfaceId)]
    public interface IShoppingItem
    {
        string Pluno { get; }
        double Amount { get; }
        double Price { get; }
    }

    [Guid(ClassId), ClassInterface(ClassInterfaceType.None)]
    public class ShoppingItem : IShoppingItem
    {
        #region COM
#if COM_INTEROP_ENABLED
        public const string ClassId = "e182de49-80fa-4e0a-865e-634d297121c2";
        public const string InterfaceId = "f29f8602-4b03-4a58-a74c-4a1c032ec383";

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

        public string Pluno { get; private set; }
        public int? Kind { get; private set; }
        public double Amount { get; private set; }
        public double Price { get; private set; }

        public ShoppingItem(string pluno, double amount, double price)
            : this(pluno, amount, price, null) { }
        public ShoppingItem(ShoppingItem si)
            : this(si.Pluno, si.Amount, si.Price, si.Kind)
        { }
        public ShoppingItem(string pluno, double amount, double price, int? kind)
        {
            Pluno = pluno;
            Kind = kind;
            Amount = amount;
            Price = price;
        }

        public override bool Equals(object obj)
        {
            if (obj is ShoppingItem)
            {
                var si = obj as ShoppingItem;
                return Pluno == si.Pluno &&
                    Price == si.Price;
            }
            if (obj is ComperableProductM)
            {
                var cp = obj as ComperableProductM;
                return (cp.isPluno && Pluno == cp.ID) ||
                    (!cp.isPluno && Kind != null && Kind.ToString() == cp.ID);
            }
            return false;
        }
        public static bool operator ==(ShoppingItem si, object obj)
        {
            return si.Equals(obj);
        }
        public static bool operator !=(ShoppingItem si, object obj)
        {
            return !si.Equals(obj);
        }
        public override int GetHashCode()
        {
            try
            {
                return int.Parse(Pluno);
            }
            catch
            {
                return 0;
            }
        }

        public bool Add(ShoppingItem si)
        {
            if (si != this)
                return false;
            if (Kind == null) Kind = si.Kind;
            Amount += si.Amount;
            return true;
        }
        public double Reduce(double amount)
        {
            return Amount -= amount;
        }
    }
}
