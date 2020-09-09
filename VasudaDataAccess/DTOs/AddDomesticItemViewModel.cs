using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VasudaDataAccess.DTOs
{
    public class AddDomesticItemViewModel
    {
        [Required(ErrorMessage = "Sender name is required")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Sender phone number is required")]
        public string SenderPhoneNumber { get; set; }

        [Required(ErrorMessage = "Sender address is required")]
        public string SenderAddress { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public long Quantity { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Receiver name is required")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Receiver phone number is required")]
        public string ReceiverPhoneNumber { get; set; }

        [Required(ErrorMessage = "Receiver address is required")]
        public string ReceiverAddress { get; set; }
    }
}
