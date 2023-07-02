using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceWorkingSimulation.Dto;

public class DistancesDto
{
	[Key]
	[Column("id")]
	public int Id { get; set; }

	[Column("distance1")]
	public int Distance1 { get; set; }

	[Column("distance2")]
	public int Distance2 { get; set; }

	[Column("distance3")]
	public int Distance3 { get; set; }
}
