using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DeviceWorkingSimulation.Dto;

public class EventDto
{
	[Key]
	[Column("id")]
	public int Id { get; set; }

	[ForeignKey("object_id")]
	public int ObjectId { get; set; }

	[Column("timestamp")]
	public DateTime CreationDate { get; set; }

	[Column("x_coordinate")]
	public float XCoordinate { get; set; }

	[Column("y_coordinate")]
	public float YCoordinate { get; set; }

	[Column("tag")]
	public string? Tag { get; set; }
}