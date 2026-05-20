using LibraryOpitech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryOpitech.Infrastructure.Persistence.Configurations;

public class BookUnitConfiguration : IEntityTypeConfiguration<BookUnit>
{
    public void Configure(EntityTypeBuilder<BookUnit> builder)
    {
        builder.ToTable("BookUnits");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasOne(x => x.Book)
            .WithMany(x => x.Units)
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
