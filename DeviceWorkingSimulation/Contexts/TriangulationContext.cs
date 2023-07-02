using DeviceWorkingSimulation.Dto;
using Microsoft.EntityFrameworkCore;


namespace DeviceWorkingSimulation.Contexts;

public class TriangulationContext : DbContext
{
	public DbSet<DistancesDto> Distances { get; set; } = null!;

	public TriangulationContext() : base()
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string pathToDb = Path.Combine("etc", "distances.db");
		optionsBuilder.UseSqlite($"Data Source={pathToDb}");
	}
}
