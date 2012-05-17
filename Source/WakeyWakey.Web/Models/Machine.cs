using System.ComponentModel.DataAnnotations;

namespace WakeyWakey.Web.Models
{
    public class Machine
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(256)]
        public string HostName { get; set; }

        [StringLength(20)]
        public string MacAddress { get; set; }

    }
}