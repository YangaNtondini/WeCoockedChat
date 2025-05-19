using System.ComponentModel.DataAnnotations;
namespace WeCoockedChat.Domain.Common
{
	public class EntityBase
	{
		public int Id { get; set; }

		public int Rev { get; set; } = 1;
		public bool IsActive { get; set; } = true;

		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		public DateTime? ModifiedOn { get; set; }

		[Timestamp]
		public byte[] RowVersion { get; set; } = null!;
	}
}
