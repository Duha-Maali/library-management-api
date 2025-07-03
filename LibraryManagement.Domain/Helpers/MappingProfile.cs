using AutoMapper;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.Models;

namespace LibraryManagement.Business.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookVM>().ReverseMap();
        CreateMap<Borrow, BorrowVM>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<BorrowVM, Borrow>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<BorrowStatus>(src.Status)));

        CreateMap<Borrower, BorrowerVM>().ReverseMap();
        CreateMap<Category, CategoryVM>().ReverseMap();

        CreateMap<CreateBorrowVM, Borrow>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BorrowStatus.Borrowed))
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.ReturnDate, opt => opt.Ignore())
            .ForMember(dest => dest.BorrowId, opt => opt.Ignore());

        CreateMap<UpdateBorrowVM, Borrow>()
            .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
            .ForAllOtherMembers(opt => opt.Ignore());
    }
}