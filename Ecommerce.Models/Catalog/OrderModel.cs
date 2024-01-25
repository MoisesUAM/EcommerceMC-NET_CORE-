using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Catalog
{
    public class OrderModel
    {
        public int IdOrder { get; set; }

        [Required]
        public string? IdUser { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }

        [Required]
        public double TotalOrderAmount { get; set; }
        [Required]
        public string? OrderState { get; set; }
        public string? PaymentState { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime MaxPaymentDate { get; set; }
        public string? IdTransaction { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ClientName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SessionId { get; set; }

        //navegaciones
        public UserModel? Users { get; set; }
    }
}
