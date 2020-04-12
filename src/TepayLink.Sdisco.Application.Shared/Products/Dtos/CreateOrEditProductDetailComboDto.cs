
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Products.Dtos
{
    public class CreateOrEditProductDetailComboDto : EntityDto<long?>
    {

		public long? RoomId { get; set; }
		
		
		public string Description { get; set; }
		
		
		 public long? ProductId { get; set; }
		 
		 		 public long? ProductDetailId { get; set; }
		 
		 		 public long? ItemId { get; set; }
		 
		 
    }
}