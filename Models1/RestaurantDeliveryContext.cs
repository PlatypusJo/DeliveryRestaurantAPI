using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackAPI.Models1;

public partial class RestaurantDeliveryContext : IdentityDbContext<User>
{
    public RestaurantDeliveryContext()
    {
    }

    public RestaurantDeliveryContext(DbContextOptions<RestaurantDeliveryContext> options)
        : base(options)
    {
    }

    #region DbSets
    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishOrder> DishOrders { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientString> IngredientStrings { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    => optionsBuilder.UseSqlServer("Server=LAPTOP-8MVHVCQA\\SQLEXPRESS;Database=RestaurantDelivery;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Category_Name");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.ToTable("Dish");

            entity.Property(e => e.DishId).HasColumnName("Dish_ID");
            entity.Property(e => e.CategoryFk).HasColumnName("Category_FK");
            entity.Property(e => e.DishCost).HasColumnName("Dish_Cost");
            entity.Property(e => e.DishGrammers).HasColumnName("Dish_Grammers");
            entity.Property(e => e.DishImage)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Dish_Image");
            entity.Property(e => e.DishName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Dish_Name");

            entity.HasOne(d => d.CategoryFkNavigation).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.CategoryFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dish_Category");
        });

        modelBuilder.Entity<DishOrder>(entity =>
        {
            entity.ToTable("DishOrder");

            entity.Property(e => e.DishOrderId).HasColumnName("DIshOrder_Id");
            entity.Property(e => e.DishFk).HasColumnName("Dish_FK");
            entity.Property(e => e.OrderFk).HasColumnName("Order_FK");

            entity.HasOne(d => d.DishFkNavigation).WithMany(p => p.DishOrders)
                .HasForeignKey(d => d.DishFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DishOrder_Dish");

            entity.HasOne(d => d.OrderFkNavigation).WithMany(p => p.DishOrders)
                .HasForeignKey(d => d.OrderFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DishOrder_Order");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.ToTable("Ingredient");

            entity.Property(e => e.IngredientId).HasColumnName("Ingredient_ID");
            entity.Property(e => e.IngredientName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ingredient_Name");
        });

        modelBuilder.Entity<IngredientString>(entity =>
        {
            entity.ToTable("IngredientString");

            entity.Property(e => e.IngredientStringId).HasColumnName("IngredientString_ID");
            entity.Property(e => e.DishFk).HasColumnName("Dish_FK");
            entity.Property(e => e.IngredientFk).HasColumnName("Ingredient_FK");

            entity.HasOne(d => d.DishFkNavigation).WithMany(p => p.IngredientStrings)
                .HasForeignKey(d => d.DishFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IngredientString_Dish");

            entity.HasOne(d => d.IngredientFkNavigation).WithMany(p => p.IngredientStrings)
                .HasForeignKey(d => d.IngredientFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IngredientString_Ingredient");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("Order_Date");
            entity.Property(e => e.OrderNumber).HasColumnName("Order_Number");
            entity.Property(e => e.UserFk).HasColumnName("User_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
