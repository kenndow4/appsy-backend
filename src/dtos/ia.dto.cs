using System.ComponentModel.DataAnnotations;
namespace appsy.src.dtos;
public class IADto {
    [Required]
    public string Msg {get;set;} = string.Empty;
}