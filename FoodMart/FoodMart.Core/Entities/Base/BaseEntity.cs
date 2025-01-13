using System;
namespace FoodMart.Core.Entities.Base;
public abstract class BaseEntity
{
	public int Id { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime CreatedTime { get; set; }
	public DateTime? UpdatedTime{ get; set; }
}

