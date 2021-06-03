using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockportWebapp.Models
{
    public class Photo
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string AccessionNo { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal Media { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "numeric")]
        public decimal Format { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Photographer { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "numeric")]
        public decimal StorageArea { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string availabletobuy { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(1)]
        public string incopyright { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "numeric")]
        public decimal area { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(255)]
        public string title { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(10)]
        public string classno { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(2000)]
        public string description { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(50)]
        public string dateofimage { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(10)]
        public string dateentered { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(50)]
        public string staffid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? viewcount { get; set; }

        [Key]
        [Column(Order = 14, TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal idno { get; set; }

        public string imgSrc { get; set; }

        public IEnumerable<PhotoComment> Comments { get; set;}

    }


    public class PhotoComment
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal commentid { get; set; }

        [Required]
        [StringLength(10)]
        public string accessionno { get; set; }

        [Column("comment")]
        [Required]
        [StringLength(255)]
        public string comment1 { get; set; }

        [Required]
        [StringLength(50)]
        public string commentname { get; set; }

        public DateTime commentdate { get; set; }
    }

}
