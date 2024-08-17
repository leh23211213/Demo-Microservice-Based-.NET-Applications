// <YourEntity> YourEntities { get; set; }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             modelBuilder.Entity<YourEntity>()
//                 .HasKey(e => new { e.Property1, e.Property2 }); // Định nghĩa khóa chính bằng cách chỉ định các thuộc tính

//             modelBuilder.Entity<YourEntity>()
//                 .Property(e => e.Property1)
//                 .ValueGeneratedOnAdd(); // Thiết lập thuộc tính được tạo ra tự động khi thêm mới (Identity)

//             // Đối với cách xác định thứ tự của các cột trong khóa chính, bạn có thể sử dụng HasKey với HasColumnOrder:
//             modelBuilder.Entity<YourEntity>()
//                 .HasKey(e => e.Property1)
//                 .HasName("PrimaryKeyName"); // Đặt tên cho khóa chính

//             modelBuilder.Entity<YourEntity>()
//                 .Property(e => e.Property1)
//                 .HasColumnOrder(1); // Thiết lập thứ tự của cột trong khóa chính

//             // Các cấu hình khác có thể được thêm vào ở đây
//         }
    
