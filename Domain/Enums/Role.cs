using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum Role
    {
        [Display(Name = "No Role")]
        None = 0,

        [Display(Name = "Customer")]
        Customer = 1,

        [Display(Name = "Supplier")]
        Supplier = 2,

        [Display(Name = "Delivery Agent")]
        DeliveryAgent = 3,

        [Display(Name = "Inventory Manager")]
        InventoryManager = 4,

        [Display(Name = "Customer Service")]
        CustomerService = 5,

        [Display(Name = "Manager")]
        Manager = 6,

        [Display(Name = "Administrator")]
        Admin = 7
    }

}
