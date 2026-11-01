using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chincisan_Laura_Lab2.Models
{
    public class Publisher
    {
        public int ID { get; set; }

        public string PublisherName { get; set; }

        [ScaffoldColumn(false)]
        public ICollection<Book>? Books { get; set; }
    }
}