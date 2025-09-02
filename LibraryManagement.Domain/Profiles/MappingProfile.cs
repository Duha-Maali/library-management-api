using AutoMapper;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.Models;

namespace LibraryManagement.Business.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Book mappings
        CreateMap<Book, BookVM>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

        CreateMap<CreateBookVM, Book>()
            .ForMember(dest => dest.BorrowedCopies, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())  
            .ForMember(dest => dest.BookId, opt => opt.Ignore());

        // Borrow mappings
        CreateMap<Borrow, BorrowVM>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.BorrowerName, opt => opt.MapFrom(src => src.Borrower.Name))
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));

        CreateMap<BorrowVM, Borrow>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<BorrowStatus>(src.Status)));

        CreateMap<CreateBorrowVM, Borrow>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BorrowStatus.Borrowed))
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.ReturnDate, opt => opt.Ignore())
            .ForMember(dest => dest.BorrowId, opt => opt.Ignore());

        CreateMap<UpdateBorrowVM, Borrow>()
            .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
            .ForAllOtherMembers(opt => opt.Ignore());

        // Borrower mappings
        CreateMap<Borrower, BorrowerVM>();

        CreateMap<CreateBorrowerVM, Borrower>()
            .ForMember(dest => dest.BorrowerId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        // Category mappings
        CreateMap<Category, CategoryVM>();

        CreateMap<CreateCategoryVM, Category>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore());
    }
}
