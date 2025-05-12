/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvoicingSys.CoreApi.Core.Entities.Collaborators.PersonalChanges;

public class PersonalChangeConfiguration : IEntityTypeConfiguration<PersonalChange>
{
    public void Configure(EntityTypeBuilder<PersonalChange> builder)
    {
        builder.ToTable("personal_changes"); // Map to table

        builder.HasKey(p => p.Id); // Primary key
        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.EffectDate)
            .HasColumnName("effect_date")
            .IsRequired();

        builder.Property(p => p.GroupHireDate)
            .HasColumnName("group_hired_date")
            .IsRequired();

        builder.Property(p => p.HiredDate)
            .HasColumnName("hired_date")
            .IsRequired();

        builder.Property(p => p.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(p => p.YearsOfService)
            .HasColumnName("years_of_service")
            .IsRequired();

        builder.Property(p => p.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(p => p.ProjectedEndDate)
            .HasColumnName("projected_end_date")
            .IsRequired();

        builder.HasOne(p => p.Collaborator)
            .WithMany() // Define the relationship with Collaborator
            .HasForeignKey("collaborator");

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey("category")
            .IsRequired();
    }
}*/