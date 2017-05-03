using AutoMapper;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.Models
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile()
        {
            CreateMap<Camp, CampModel>()
                .ForMember(c => c.StartDate, opt => opt.MapFrom(camp => camp.EventDate))
                .ForMember(c => c.EndDate,
                    opt => opt.ResolveUsing(camp => camp.EventDate.AddDays(camp.Length - 1)))
                .ForMember(c => c.Url,
                    opt => opt.ResolveUsing<CampUrlResolver>())
                .ReverseMap()
                .ForMember(camp => camp.EventDate, opt => opt.MapFrom(c => c.StartDate))
                .ForMember(camp => camp.Length,
                    opt => opt.ResolveUsing(c => (c.EndDate - c.StartDate).Days + 1))
                .ForMember(camp => camp.Location,
                    opt => opt.ResolveUsing(c => new Location
                    {
                        Address1 = c.LocationAddress1,
                        Address2 = c.LocationAddress2,
                        Address3 = c.LocationAddress3,
                        CityTown = c.LocationCityTown,
                        StateProvince = c.LocationStateProvince,
                        PostalCode = c.LocationPostalCode,
                        Country = c.LocationCountry
                    }));
        }
    }
}
