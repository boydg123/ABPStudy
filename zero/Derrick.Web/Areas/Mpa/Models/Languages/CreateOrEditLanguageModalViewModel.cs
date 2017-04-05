using Abp.AutoMapper;
using Derrick.Localization.Dto;

namespace Derrick.Web.Areas.Mpa.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode
        {
            get { return Language.Id.HasValue; }
        }

        public CreateOrEditLanguageModalViewModel(GetLanguageForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}