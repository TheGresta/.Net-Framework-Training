using Entities.Concrete;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductsListed = "Ürünler listelendi";
        public static string ProductCountOfCategoryError = "Aynı ünün id sine sahip max 10 ürün olabilir";
        public static string ProductNameAlreadyExists = "Ürün ismi sistemde zaten var";
        public static string CategoryCountOutOfLimit = "Kategori sayısı 15 i geçemez";
        public static string AuthorizationDenied = "Yetki Yok";
    }
}
