using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Shipping.Core.Integration.Dtos;

public class GetByIdAndAddressIdDto
{
    public int MemberId { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public string City { get; set; }
    public string County { get; set; }
    public string Town { get; set; }
    public string BuildingNo { get; set; }
    public string FlatNumber { get; set; }
    public string Street { get; set; }

    public string Address { get; set; }
}
