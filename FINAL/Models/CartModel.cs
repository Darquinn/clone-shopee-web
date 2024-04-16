using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FINAL.Models
{
  
    public class CartModel
    {
        List<CartModel> items = new List<CartModel>();
        public IEnumerable<CartModel> Items
        {
            get { return items; }
        }
        public Products _product { get; set; }
        public int _quantity { get; set; }


        public int CartID { get; set; }
        public string CartName { get; set; }
        public string CartSupplier { get; set; }
        public Nullable<decimal> CartPrice { get; set; }
        public Nullable<decimal> CartTotalCost { get; set; }
        public string CartImage { get; set; }
        public int CartQuantity { get; set; }

        public CartModel(int id)
        {
            using (ShopeeEntities db = new ShopeeEntities())
            {
                Products product = db.Products.Single(n => n.ProID == id);
                this.CartID = id;
                this.CartName = product.ProName;
                this.CartSupplier = product.Suppliers.SupName;
                this.CartPrice = product.Price.Value;
                this.CartQuantity = 1;
                this.CartTotalCost = CartPrice * CartQuantity;
                this.CartImage = product.ProImage;
            }
        }

        public CartModel(int id, int sl)
        {
            using (ShopeeEntities db = new ShopeeEntities())
            {
                Products product = db.Products.Single(n => n.ProID == id);
                this.CartID = id;
                this.CartName = product.ProName;
                this.CartSupplier = product.Suppliers.SupName;
                this.CartPrice = product.Price.Value;
                this.CartTotalCost = CartPrice * CartQuantity;
                this.CartImage = product.ProImage;
                this.CartQuantity = sl;
            }
        }

        public CartModel()
        {

        }

        public void ClearCart()
        {
            items.Clear();
        }


    }
}