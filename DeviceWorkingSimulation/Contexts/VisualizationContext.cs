using Microsoft.EntityFrameworkCore;

using DeviceWorkingSimulation.Dto;


namespace DeviceWorkingSimulation.Contexts;

public class VisualizationContext : DbContext
{
	public DbSet<EventDto> Events { get; set; } = null!;

	public VisualizationContext() : base()
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		//string pathToDb = Path.Combine("F:", "etc", "testDb.db");
		optionsBuilder.UseSqlite($"Data Source=F:\\etc\\testDb.db");
	}
}
