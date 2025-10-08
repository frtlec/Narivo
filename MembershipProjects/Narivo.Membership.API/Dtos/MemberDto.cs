using Narivo.Membership.API.Infastructure.Entities;
using Narivo.Shared.Constants.Enums;
using Narivo.Shared.Dtos;
using System.IO;
using System.Text.Json.Serialization;

namespace Narivo.Membership.API.Dtos;


public class MemberDto : DtoBase
{
    public string Name { get; set; }
    public string SurName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
    public List<CardDto> Cards { get; set; } = new List<CardDto>();

    [JsonConstructor]
    public MemberDto()
    {

    }
    public MemberDto(Member member)
    {
        Id = member.Id;
        Name = member.Name;
        SurName = member.SurName;
        Email = member.Email;
        PhoneNumber = member.PhoneNumber;
        Addresses = member.Addresses.Select(f => new AddressDto
        {
            Id = f.Id,
            City = f.City,
            County = f.County,
            Town = f.Town,
            BuildingNo = f.BuildingNo,
            FlatNumber = f.FlatNumber,
            IsDefault = f.IsDefault,
            CreatedAt = f.CreatedAt,
            UpdatedAt = f.UpdatedAt
        }).ToList();

        Cards = member.Carts.Select(f => new CardDto
        {
            Id = f.Id,
            HolderName = f.HolderName,
            No = f.No,
            CVV = f.CVV,
            Year = f.Year,
            Month = f.Month,
            Bank = f.Bank,
            IsDefault = f.IsDefault,
            CreatedAt = f.CreatedAt,
            UpdatedAt = f.UpdatedAt
        }).ToList();

        CreatedAt = member.CreatedAt;
        UpdatedAt = member.UpdatedAt;
    }
}

public class AddressDto : DtoBase
{
    public string City { get; set; }
    public string County { get; set; }
    public string Town { get; set; }
    public string Street { get; set; }
    public string BuildingNo { get; set; }
    public string FlatNumber { get; set; }
    public bool IsDefault { get; set; }

    [JsonConstructor]
    public AddressDto()
    {

    }

    public AddressDto(Address address)
    {
        City = address.City;
        County = address.County;
        Town = address.Town;
        Street = address.Street;
        BuildingNo = address.BuildingNo;
        FlatNumber = address.FlatNumber;
        IsDefault = address.IsDefault;
        CreatedAt = address.CreatedAt;
        UpdatedAt = address.UpdatedAt;

    }
}

public class CardDto : DtoBase
{
    public string HolderName { get; set; }
    public string No { get; set; }
    public string CVV { get; set; }
    public string Year { get; set; }
    public string Month { get; set; }
    public Bank Bank { get; set; }
    public bool IsDefault { get; set; }

    [JsonConstructor]
    public CardDto()
    {

    }
}


public class GetByIdAndAddressIdDto
{
    public GetByIdAndAddressIdDto(Member member, Address address)
    {
        MemberId = member.Id;
        Name = member.Name;
        SurName = member.SurName;
        Email = member.Email;
        PhoneNumber = member.PhoneNumber;

        City = address.City;
        County = address.County;
        Town = address.Town;
        BuildingNo = address.BuildingNo;
        FlatNumber = address.FlatNumber;
        Street = address.Street;
    }
    public int MemberId { get; }
    public string Name { get; }
    public string SurName { get; }
    public string Email { get; }
    public string PhoneNumber { get; }

    public string City { get; }
    public string County { get; }
    public string Town { get; }
    public string BuildingNo { get; }
    public string FlatNumber { get; }
    public string Street { get; }

    public string Address => $"{Street} {BuildingNo}/{FlatNumber}, {County}/{City}";


}